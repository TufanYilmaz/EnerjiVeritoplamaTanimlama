using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperFilm.Enerji.WebUI.ViewModels.AccountViewModels;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //IdentityUser
            }
            return Ok();
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
