using System.Threading.Tasks;

namespace ServerHost.Hubs.Chat
{
    public interface IChatHub
    {
        Task ReceiveChats(string result);
    }
}