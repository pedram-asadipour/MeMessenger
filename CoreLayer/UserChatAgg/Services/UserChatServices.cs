using System;
using System.Collections.Generic;
using System.Linq;
using _Framework.Auth;
using CoreLayer.UserChatAgg.Contract;
using DataLayer.Entities;
using DataLayer.UnitOfWork;

namespace CoreLayer.UserChatAgg.Services
{
    public class UserChatServices : IUserChatServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthHelper _authHelper;

        public UserChatServices(IUnitOfWork unitOfWork, IAuthHelper authHelper)
        {
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
        }

        public void JoinChat(JoinChat command)
        {
            try
            {
                var chat = new UserChat(command.ChatId,command.AccountId,command.Permission);
                _unitOfWork.UserChat.Create(chat);
                _unitOfWork.SaveChange();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void JoinChat(List<JoinChat> command)
        {
            try
            {
                var userChats = new List<UserChat>();

                command.ForEach(x =>
                {
                    userChats.Add(new UserChat(x.ChatId,x.AccountId,x.Permission));
                });

                _unitOfWork.UserChat.Create(userChats);
                _unitOfWork.SaveChange();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<string> GetUsersChat(long chatId)
        {
            var accountId = _authHelper.GetAuthAccount().Id;

            var query = _unitOfWork.UserChat.Get()
                .Where(x => x.ChatId == chatId && x.AccountId != accountId)
                .Select(x => new string(x.AccountId.ToString()))
                .ToList();
            return query;
        }
    }
}