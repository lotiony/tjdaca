using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace tjdaca.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string id, string pw)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, id),
                new Claim(ClaimTypes.Role, "admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
            return LocalRedirect(Url.Content("~/"));
        }
    }
}
