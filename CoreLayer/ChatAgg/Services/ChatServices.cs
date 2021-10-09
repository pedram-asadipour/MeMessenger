using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using _Framework;
using _Framework.Auth;
using CoreLayer.ChatAgg.Contract;
using DataLayer.Entities;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.ChatAgg.Services
{
    public class ChatServices : IChatServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthHelper _authHelper;

        public ChatServices(IUnitOfWork unitOfWork, IAuthHelper authHelper)
        {
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
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
                    .ToList()
                    .Select(x => new ChatViewModel
                    {
                        ChatId = x.ChatId,
                        Title = x.Chat.Title,
                        Image = x.Chat.Image,
                        LastMessage = x.Chat.Messages.FirstOrDefault()?.Body ?? "",
                        LastMessageDate = x.Chat.Messages.FirstOrDefault()?.CreationDate.ToPersianDateTime() ?? "",
                        IsPrivate = x.Chat.IsPrivate,
                        IsGroup = x.Chat.IsGroup,
                        IsChannel = x.Chat.IsChannel,
                        IsOnline = false,
                    }).ToList();

                var privateAccounts = _unitOfWork.UserChat.Get()
                    .Include(x => x.Account)
                    .Select(x => new {x.ChatId, x.AccountId, x.Account.Username, x.Account.ProfileImage})
                    .ToList();

                foreach (var chat in query.Where(x => x.IsPrivate))
                {
                    chat.AccountId = privateAccounts
                        .SingleOrDefault(x => x.ChatId == chat.ChatId && x.AccountId != accountId).AccountId;

                    chat.Title = privateAccounts
                        .SingleOrDefault(x => x.ChatId == chat.ChatId && x.AccountId != accountId)?.Username;

                    chat.Image = privateAccounts
                        .SingleOrDefault(x => x.ChatId == chat.ChatId && x.AccountId != accountId)?.ProfileImage;
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

                var channelGroupChat = _unitOfWork.Chats.Get()
                    .Where(x => x.Title.StartsWith(search))
                    .Include(x => x.Messages.OrderByDescending(x => x.Id))
                    .Select(x => new ChatViewModel
                    {
                        ChatId = x.Id,
                        Title = x.Title,
                        Image = x.Image,
                        LastMessage = x.Messages.FirstOrDefault().Body ?? "",
                        LastMessageDate = x.Messages.FirstOrDefault().CreationDate.ToPersianDateTime() ?? "",
                        IsPrivate = x.IsPrivate,
                        IsGroup = x.IsGroup,
                        IsChannel = x.IsChannel,
                        IsOnline = false
                    })
                    .ToList();

                var privateChat = _unitOfWork.Accounts.Get()
                    .Where(x => x.Username.StartsWith(search) && x.Id != accountId)
                    .Select(x => new ChatViewModel
                    {
                        ChatId = x.Id,
                        Title = x.Username,
                        Image = x.ProfileImage,
                        LastMessage = "",
                        LastMessageDate = "",
                        IsPrivate = true,
                        IsGroup = false,
                        IsChannel = false,
                        IsOnline = false
                    }).ToList();

                var query = new List<ChatViewModel>();
                query.AddRange(privateChat);
                query.AddRange(channelGroupChat);

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