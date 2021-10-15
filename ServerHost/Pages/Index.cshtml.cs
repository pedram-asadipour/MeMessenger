using System.Linq;
using CoreLayer.ChatAgg.Contract;
using CoreLayer.MessageAgg.Contract;
using CoreLayer.UserChatAgg.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ServerHost.Hubs.Chat;

namespace ServerHost.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IChatServices _chatServices;
        private readonly IMessageServices _messageServices;
        private readonly IUserChatServices _userChatServices;
        private readonly IHubContext<ChatHub> _hubContext;

        public IndexModel(IChatServices chatServices, IMessageServices messageServices, IHubContext<ChatHub> hubContext, IUserChatServices userChatServices)
        {
            _chatServices = chatServices;
            _messageServices = messageServices;
            _hubContext = hubContext;
            _userChatServices = userChatServices;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostGetChats()
        {
            var result = _chatServices.GetChats();
            return new JsonResult(result);
        }

        public JsonResult OnPostSearchChats([FromBody]string search)
        {
            var result = _chatServices.SearchChats(search);
            return new JsonResult(result);
        }

        public JsonResult OnPostGetChatMessages([FromBody] long id)
        {
            var result = _messageServices.GetChatMessages(id);
            _hubContext.Groups.RemoveFromGroupAsync(HttpContext.Connection.Id, result.First().ChatId.ToString());
            _hubContext.Groups.AddToGroupAsync(HttpContext.Connection.Id, result.First().ChatId.ToString());
            return new JsonResult(result);
        }

        public JsonResult OnPostSendMessage([FromForm]SendMessage command)
        {
            var result = _messageServices.SendMessage(command);
            var usersChat = _userChatServices.GetUsersChat(result.ChatId);

            result.IsOwner = false;
            _hubContext.Clients.Users(usersChat).SendAsync("ReceiveMessage", result);

            result.IsOwner = true;
            return new JsonResult(result);
        }
    }
}