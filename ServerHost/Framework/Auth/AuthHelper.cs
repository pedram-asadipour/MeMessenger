using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using _Framework.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ServerHost.Framework.Auth
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void Signin(AuthViewModel command)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, command.Id.ToString()),
                new Claim(ClaimTypes.Name, command.Username),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties()
            {
                IsPersistent = command.RememberMe
            };

            _contextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);
        }

        public void Signout()
        {
            if (IsAuth())
                _contextAccessor.HttpContext
                    .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool IsAuth()
        {
            return _contextAccessor.HttpContext.User.Identity is {IsAuthenticated: true};
        }

        public AuthViewModel GetAuthAccount()
        {
            if (!IsAuth())
                return new AuthViewModel();

            var id = _contextAccessor.HttpContext
                .User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var username = _contextAccessor.HttpContext
                .User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var result = new AuthViewModel(int.Parse(id), username);

            return result;
        }
    }
}