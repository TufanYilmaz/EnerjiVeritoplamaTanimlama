using Microsoft.AspNetCore.Mvc;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class IsletmeSayacDagilimiController(
        ILogger<IsletmeSayacDagilimiController> _logger,
        IRepository _repository,
        IQueryRepository<EnerjiDbContext> _queryRepository,
        IRepository<EnerjiDbContext> _enerjiRepository) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
