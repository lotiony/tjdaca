using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace tjdaca.Pages
{
    public class LogoutModel : PageModel
    {
		public async Task<IActionResult> OnGetAsync(string id, string pw)
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return LocalRedirect(Url.Content("~/"));
		}
	}
}
