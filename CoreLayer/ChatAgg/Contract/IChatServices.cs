using System.Collections.Generic;
using _Framework;
using DataLayer.Entities;

namespace CoreLayer.ChatAgg.Contract
{
    public interface IChatServices
    {
        OperationResult CreateChat(CreateChat command);
        Chat CreatePrivateChat();
        List<ChatViewModel> GetChats();
        List<ChatViewModel> SearchChats(string search);
    }
}