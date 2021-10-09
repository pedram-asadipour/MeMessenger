using System.Collections.Generic;

namespace CoreLayer.ChatAgg.Contract
{
    public interface IChatServices
    {
        List<ChatViewModel> GetChats();
        List<ChatViewModel> SearchChats(string search);
    }
}