using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Movies.Client.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //  performs the sign out from the cookie authenticated operation from your browser
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme); // perform the sign out from the OpenID Connect authentication from IdentityServer4
        }
    }
}