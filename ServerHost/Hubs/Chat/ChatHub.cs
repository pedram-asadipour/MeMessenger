using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Framework.Auth;
using CoreLayer.ChatAgg.Contract;
using CoreLayer.UserChatAgg.Contract;
using Microsoft.AspNetCore.SignalR;

namespace ServerHost.Hubs.Chat
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IChatServices _chatServices;
        private readonly IAuthHelper _authHelper;
        private readonly List<UserStatus> _userStatus;

        public ChatHub(IChatServices chatServices, IAuthHelper authHelper, List<UserStatus> userStatus)
        {
            _chatServices = chatServices;
            _authHelper = authHelper;
            _userStatus = userStatus;
        }

        public Task UserStatus(bool isOnline)
        {
            var account = _authHelper.GetAuthAccount();

            var userStatus = new UserStatus
            {
                Id = account.Id,
                ConnectionId = Context.ConnectionId,
                Username = account.Username,
                IsOnline = isOnline
            };

            return Clients.Others.UserStatus(userStatus);
        }

        public override Task OnConnectedAsync()
        {
            AddOnlineUser();
            UserStatus(true);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            RemoveOfflineUser();
            UserStatus(false);
            return base.OnDisconnectedAsync(exception);
        }

        public void AddOnlineUser()
        {
            var user = _authHelper.GetAuthAccount();

            if (user != null)
                _userStatus.Add(new UserStatus
                {
                    Id = user.Id,
                    ConnectionId = Context.ConnectionId,
                    Username = user.Username,
                    IsOnline = true
                });
        }

        public void RemoveOfflineUser()
        {
            var userId = _authHelper.GetAuthAccount().Id;
            var user = _userStatus.Find(x => x.Id == userId);
            _userStatus.Remove(user);
        }
    }
}