using CoreLayer.ChatAgg.Contract;
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

        public IndexModel(IChatServices chatServices)
        {
            _chatServices = chatServices;
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
    }
}