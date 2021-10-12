using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using _Framework;
using _Framework.Auth;
using _Framework.FileManager;
using CoreLayer.ChatAgg.Contract;
using CoreLayer.MessageAgg.Contract;
using CoreLayer.UserChatAgg.Contract;
using DataLayer.Entities;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.MessageAgg.Services
{
    public class MessageServices : IMessageServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthHelper _authHelper;
        private readonly IChatServices _chatServices;
        private readonly IUserChatServices _userChatServices;
        private readonly IFileManager _fileManager;

        public MessageServices(IUnitOfWork unitOfWork, IAuthHelper authHelper, IChatServices chatServices, IUserChatServices userChatServices, IFileManager fileManager)
        {
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
            _chatServices = chatServices;
            _userChatServices = userChatServices;
            _fileManager = fileManager;
        }

        public List<MessageViewModel> GetChatMessages(long chatId)
        {
            try
            {
                var accountId = _authHelper.GetAuthAccount().Id;

                var query = _unitOfWork.Messages.Get()
                    .Where(x => x.ChatId == chatId)
                    .Include(x => x.Account)
                    .Select(x => new MessageViewModel
                    {
                        Id = x.Id,
                        IsOwner = x.AccountId == accountId,
                        Username = x.Account.Username,
                        ProfileImage = x.Account.ProfileImage,
                        Body = x.Body,
                        IsFile = x.IsFile,
                        SendDate = x.CreationDate.ToPersianTime()
                    })
                    .ToList();

                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public MessageViewModel SendMessage(SendMessage command)
        {
            try
            {
                var accountId = _authHelper.GetAuthAccount().Id;
                
                if (!_unitOfWork.Chats.Exists(x => x.Id == command.ChatId))
                {
                    var chat =_chatServices.CreatePrivateChat();

                    command.ChatId = chat.Id;

                    var joinChats = new List<JoinChat>()
                    {
                        new JoinChat(chat.Id, accountId, Permission.User),
                        new JoinChat(chat.Id, command.ReceiveId, Permission.User)
                    }; 
                    _userChatServices.JoinChat(joinChats);
                }
                
                if (!_unitOfWork.UserChat.Exists(x => x.ChatId == command.ChatId && x.AccountId == accountId))
                {
                    var joinChat = new JoinChat(command.ChatId,accountId,Permission.User);
                    _userChatServices.JoinChat(joinChat);
                }

                
                var body = command.Body;
                var isFile = false;
                
                if (command.File != null)
                {
                    body =_fileManager.Uploader(command.File, $"chat-{command.ChatId}");
                    isFile = true;
                }
                
                var message = new Message(command.ChatId, accountId, body, isFile);
                _unitOfWork.Messages.Create(message);
                _unitOfWork.SaveChange();
                
                var account = _unitOfWork.Accounts.GetBy(accountId);
                
                return new MessageViewModel
                {
                    Id = message.Id,
                    IsOwner = true,
                    Username = account.Username,
                    ProfileImage = account.ProfileImage,
                    Body = message.Body,
                    IsFile = message.IsFile,
                    SendDate = message.CreationDate.ToPersianTime()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}