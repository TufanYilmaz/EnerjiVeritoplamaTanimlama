using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class IsletmeEnerjiTuketimi(ILogger<HomeController> logger,
            IRepository repository,
            IQueryRepository<EnerjiDbContext> queryRepository,
            IRepository<EnerjiDbContext> enerjiRepository) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var isletmeler = await queryRepository.GetListAsync<Isletme>(includes:r=>r.Include(x=>x.Isyeri));
            IsletmeEnerjiTuketimViewModel model = new IsletmeEnerjiTuketimViewModel() 
            { 
                Isletmeler = isletmeler
            };
            return View(model);
        }
        public async Task<IActionResult> IsletmeSayacVerileri(IsletmeEnerjiTuketimRequestModel RequestModel)
        {
           
            return View();
        }

    }

}
