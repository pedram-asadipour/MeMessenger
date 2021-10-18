using System;
using System.Collections.Generic;
using System.Linq;
using _Framework;
using _Framework.Auth;
using _Framework.FileManager;
using CoreLayer.ChatAgg.Contract;
using CoreLayer.UserChatAgg.Contract;
using DataLayer.Entities;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.ChatAgg.Services
{
    public class ChatServices : IChatServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthHelper _authHelper;
        private readonly IFileManager _fileManager;
        private readonly OperationResult _result;
        private readonly List<UserStatus> _userStatus;

        public ChatServices(IUnitOfWork unitOfWork, IAuthHelper authHelper, IFileManager fileManager,
            List<UserStatus> userStatus)
        {
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
            _fileManager = fileManager;
            _userStatus = userStatus;

            _result = new OperationResult();
        }

        public ChatInfoViewModel GetChatInfo(ChatInfo command)
        {
            if (command == null)
                return new ChatInfoViewModel();

            try
            {
                if (command.ChatId != 0 && command.AccountId != 0)
                {
                    var account = _unitOfWork.Accounts.GetBy(command.AccountId);

                    return new ChatInfoViewModel
                    {
                        AccountId = account.Id,
                        Title = account.Username,
                        Image = account.ProfileImage,
                        IsOnline = _userStatus.Find(x => x.Id == account.Id)?.IsOnline ?? false,
                        IsGroupOrChannel = false
                    };
                }

                if (command.ChatId != 0 && command.AccountId == 0)
                {
                    var chat = _unitOfWork.Chats.GetBy(command.ChatId);

                    return new ChatInfoViewModel
                    {
                        AccountId = 0,
                        Title = chat.Title,
                        Image = chat.Image,
                        IsOnline = false,
                        IsGroupOrChannel = true
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return new ChatInfoViewModel();
        }

        public OperationResult CreateChat(CreateChat command)
        {
            try
            {
                if (_unitOfWork.Chats.Exists(x => x.Title == command.Title && x.Title != ""))
                    _result.Failed(OperationMessage.ExistTitle);

                var image = "";

                if (command.Image != null)
                    image = _fileManager.Uploader(command.Image, "Chat");

                var chat = new Chat(command.Title, image, command.IsPrivate, command.IsGroup, command.IsChannel);
                _unitOfWork.Chats.Create(chat);
                _unitOfWork.SaveChange();

                return _result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Chat CreatePrivateChat()
        {
            try
            {
                var chat = new Chat("", "", true, false, false);
                _unitOfWork.Chats.Create(chat);
                _unitOfWork.SaveChange();

                return chat;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<ChatViewModel> GetChats()
        {
            try
            {
                var accountId = _authHelper.GetAuthAccount().Id;

                var query = _unitOfWork.UserChat.Get()
                    .Where(x => x.AccountId == accountId)
                    .Include(x => x.Chat)
                    .ThenInclude(x => x.Messages.OrderByDescending(x => x.Id))
                    .OrderByDescending(x => x.ChatId)
                    .Select(x => new {x.ChatId, x.Chat})
                    .ToList()
                    .Select(x => new ChatViewModel
                    {
                        ChatId = x.ChatId,
                        Title = x.Chat.Title,
                        Image = x.Chat.Image,
                        LastMessage = (x.Chat.Messages.FirstOrDefault()?.Body != null 
                            ? ((x.Chat.Messages.FirstOrDefault().IsFile) ? "فایل" : x.Chat.Messages.FirstOrDefault()?.Body) 
                            : ""),
                        LastMessageDate = x.Chat.Messages.FirstOrDefault()?.CreationDate.ToPersianDateTime() ?? "",
                        IsPrivate = x.Chat.IsPrivate,
                        IsGroup = x.Chat.IsGroup,
                        IsChannel = x.Chat.IsChannel,
                        IsOnline = false,
                    })
                    .ToList();

                var privateAccounts = _unitOfWork.UserChat.Get()
                    .Include(x => x.Account)
                    .Select(x => new {x.ChatId, x.AccountId, x.Account.Username, x.Account.ProfileImage})
                    .OrderByDescending(x => x.ChatId)
                    .ToList();

                foreach (var chat in query.Where(x => x.IsPrivate))
                {
                    chat.AccountId = privateAccounts
                        .SingleOrDefault(x => x.ChatId == chat.ChatId && x.AccountId != accountId)?.AccountId ?? 0;

                    chat.Title = privateAccounts
                        .SingleOrDefault(x => x.ChatId == chat.ChatId && x.AccountId != accountId)?.Username;

                    chat.Image = privateAccounts
                        .SingleOrDefault(x => x.ChatId == chat.ChatId && x.AccountId != accountId)?.ProfileImage;

                    chat.IsOnline = _userStatus.Find(x => x.Id == chat.AccountId)?.IsOnline ?? false;
                }

                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public List<ChatViewModel> SearchChats(string search)
        {
            try
            {
                var accountId = _authHelper.GetAuthAccount().Id;

                var query = new List<ChatViewModel>();

                if (string.IsNullOrWhiteSpace(search)) return query;

                query.AddRange(_unitOfWork.Chats.Get()
                    .Where(x => x.Title.StartsWith(search))
                    .Include(x => x.Messages.OrderByDescending(x => x.Id))
                    .Select(x => new ChatViewModel
                    {
                        ChatId = x.Id,
                        Title = x.Title,
                        Image = x.Image,
                        LastMessage = x.Messages.First().Body ?? "",
                        LastMessageDate = x.Messages.First().CreationDate == null
                            ? ""
                            : x.Messages.First().CreationDate.ToPersianDateTime(),
                        IsPrivate = x.IsPrivate,
                        IsGroup = x.IsGroup,
                        IsChannel = x.IsChannel,
                        IsOnline = false
                    })
                    .ToList());

                var chats = _unitOfWork.Chats.Get()
                    .Where(x => x.IsPrivate)
                    .Include(x => x.UserChats)
                    .Select(x => new {x.Id, x.UserChats})
                    .ToList();

                var privateChats = _unitOfWork.Accounts.Get()
                    .Where(x => x.Username.StartsWith(search) && x.Id != accountId)
                    .Include(x => x.UserChats)
                    .Select(x => new ChatViewModel
                    {
                        AccountId = x.Id,
                        Title = x.Username,
                        Image = x.ProfileImage,
                        LastMessage = "",
                        LastMessageDate = "",
                        IsPrivate = true,
                        IsGroup = false,
                        IsChannel = false,
                        IsOnline = false,
                    })
                    .ToList();

                var chat2 = new List<Chat>();

                foreach (var chat in privateChats)
                {
                    var currentChat = chats.SingleOrDefault(x =>
                        x.UserChats.Any(z => z.AccountId == chat.AccountId) &&
                        x.UserChats.Any(z => z.AccountId == accountId));
                    chat.ChatId = currentChat?.Id ?? 0;
                    chat.IsOnline = _userStatus.Find(x => x.Id == chat.AccountId)?.IsOnline ?? false;
                }

                query.AddRange(privateChats);

                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}