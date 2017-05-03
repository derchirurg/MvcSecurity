using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using SecurityApp.Models;

namespace SecurityApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SecurityController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel() {ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == model.Password)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim("sub", model.Username.GetHashCode().ToString()),
                        new Claim(ClaimTypes.Name, model.Username.ToLower()),
                        new Claim(ClaimTypes.Email, "foo@ding.de"),
                    }, "password");

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.Authentication.SignInAsync("MyCookies", principal);
                    return LocalRedirect(model.ReturnUrl);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("MyCookies");
            return Redirect("~/");
        }


        public IActionResult Google(string returnUrl = "~/")
        {
            var options = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("External"),
                Items = { { "returnUrl", returnUrl } }
            };
            return Challenge(options, "Google");
        }

        public async Task<IActionResult> External()
        {
            var authInfo = await HttpContext.Authentication.GetAuthenticateInfoAsync("External");
            if (authInfo == null)
            {
                return NotFound();
            }

            var returnUrl = "~/";
            authInfo.Properties.Items.TryGetValue("returnUrl", out returnUrl);

            await HttpContext.Authentication.SignInAsync("MyCookies", authInfo.Principal);
            await HttpContext.Authentication.SignOutAsync("External");

            return LocalRedirect(returnUrl);
        }


        [HttpGet]
        public IActionResult Nope(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}