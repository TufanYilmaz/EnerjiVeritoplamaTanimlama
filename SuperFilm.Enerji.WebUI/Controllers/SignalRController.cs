using Microsoft.AspNetCore.Mvc;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class SignalRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
