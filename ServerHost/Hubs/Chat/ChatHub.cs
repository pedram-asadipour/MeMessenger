using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLayer.ChatAgg.Contract;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ServerHost.Hubs.Chat
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IChatServices _chatServices;

        public ChatHub(IChatServices chatServices)
        {
            _chatServices = chatServices;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }

    public interface IChatHub
    {
        Task ReceiveChats(string result);
    }
}