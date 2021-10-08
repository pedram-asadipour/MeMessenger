using _Framework.Auth;
using CoreLayer.AccountAgg.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServerHost.Pages
{
    public class AuthModel : PageModel
    {
        private readonly IAccountServices _accountServices;
        private readonly IAuthHelper _authHelper;

        public AuthModel(IAccountServices accountServices, IAuthHelper authHelper)
        {
            _accountServices = accountServices;
            _authHelper = authHelper;
        }

        public IActionResult OnGet()
        {
            if (_authHelper.IsAuth())
                return Redirect("/Index");

            return Page();
        }

        public IActionResult OnPostSignin([FromForm] SigninViewModel command)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _accountServices.Signin(command);

            if (result.IsSucceeded)
                return Redirect("/Index");

            ModelState.AddModelError(command.Username,result.Message);
            return Page();
        }

        public IActionResult OnPostSignup([FromForm] SignupViewModel command)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _accountServices.Signup(command);

            if (result.IsSucceeded)
            {
                ModelState.AddModelError(command.Username, result.Message);
                return Page();
            }

            ModelState.AddModelError(command.Username, result.Message);
            return Page();
        }

        public IActionResult OnGetSignout()
        {
            _accountServices.Signout();
            return Page();
        }
    }
}
