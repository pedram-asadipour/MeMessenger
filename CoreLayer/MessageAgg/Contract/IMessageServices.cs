using System.Collections.Generic;

namespace CoreLayer.MessageAgg.Contract
{
    public interface IMessageServices
    {
        List<MessageViewModel> GetChatMessages(long chatId);
        MessageViewModel SendMessage(SendMessage command);
    }
}