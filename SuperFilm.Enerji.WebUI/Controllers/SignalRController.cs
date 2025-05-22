using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    [Authorize]
    public class SignalRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
