using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using _Framework;
using _Framework.Auth;
using CoreLayer.AccountAgg.Contract;
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
        private readonly IAccountServices _accountServices;
        private readonly IAuthHelper _authHelper;
        private readonly IHubContext<ChatHub> _hubContext;

        public IndexModel(IChatServices chatServices, IMessageServices messageServices, IHubContext<ChatHub> hubContext, IUserChatServices userChatServices, IAccountServices accountServices, IAuthHelper authHelper)
        {
            _chatServices = chatServices;
            _messageServices = messageServices;
            _hubContext = hubContext;
            _userChatServices = userChatServices;
            _accountServices = accountServices;
            _authHelper = authHelper;
        }

        public void OnGet()
        {
        }

        public PartialViewResult OnGetProfile()
        {
            var profile = _accountServices.GetCurrentAccount();
            return Partial("Shared/Home/_userInfo",profile);
        }

        public JsonResult OnPostEditProfile([FromForm] ProfileViewModel command)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new OperationResult().Failed(OperationMessage.AllRequired));

            var result = _accountServices.EditAccount(command);
            return new JsonResult(result);
        }

        public PartialViewResult OnGetPassword()
        {
            var editPassword = new EditPassword()
            {
                Id = _authHelper.GetAuthAccount().Id
            };
            return Partial("Shared/Home/_editPassword",editPassword);
        }

        public JsonResult OnPostEditPassword([FromForm]EditPassword command)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new OperationResult().Failed(OperationMessage.AllRequired));

            var result = _accountServices.EditPassword(command);
            return new JsonResult(result);
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
            if (id == 0)
                return new JsonResult("");

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

        public JsonResult OnPostGetChatInfo([FromBody]ChatInfo command)
        {
            var result = _chatServices.GetChatInfo(command);
            return new JsonResult(result);
        }

        public JsonResult OnPostGetProfileImage()
        {
            var result = _accountServices.GetProfileImage();
            return new JsonResult(result);
        }
    }
}