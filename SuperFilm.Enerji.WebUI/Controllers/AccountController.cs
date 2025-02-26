using Microsoft.AspNetCore.Mvc;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
