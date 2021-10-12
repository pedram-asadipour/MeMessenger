using System.Collections.Generic;
using DataLayer.Entities;

namespace CoreLayer.UserChatAgg.Contract
{
    public interface IUserChatServices
    {
        void JoinChat(JoinChat command);
        void JoinChat(List<JoinChat> command);
    }
}