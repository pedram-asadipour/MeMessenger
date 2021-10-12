using CoreLayer.ChatAgg.Contract;
using CoreLayer.MessageAgg.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServerHost.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IChatServices _chatServices;
        private readonly IMessageServices _messageServices;

        public IndexModel(IChatServices chatServices, IMessageServices messageServices)
        {
            _chatServices = chatServices;
            _messageServices = messageServices;
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
            return new JsonResult(result);
        }

        public JsonResult OnPostSendMessage([FromForm]SendMessage command)
        {
            var result = _messageServices.SendMessage(command);
            return new JsonResult(result);
        }
    }
}