using System.Threading.Tasks;
using CoreLayer.UserChatAgg.Contract;

namespace ServerHost.Hubs.Chat
{
    public interface IChatHub
    {
        Task UserStatus(UserStatus command);
    }
}