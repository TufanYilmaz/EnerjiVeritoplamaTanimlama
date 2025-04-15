using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class IsletmeSayacDagilimiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public IsletmeSayacDagilimiController(ILogger<HomeController> logger,
            IRepository repository,
            IQueryRepository<EnerjiDbContext> queryRepository,
            IRepository<EnerjiDbContext> enerjiRepository)
        {
            _logger = logger;
            _repository = repository;
            _queryRepository = queryRepository;
            _enerjiRepository = enerjiRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _queryRepository.GetQueryable<IsletmeSayacDagilimi>()
                                      .Include(isd => isd.Isletme)
                                      .Include(isd => isd.Sayac)
                                      .ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> AddIsletmeSayac()
        {
            var isletme = await _queryRepository.GetQueryable<Isletme>().ToListAsync();
            var sayactanimlari = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            var model = new IsletmeSayacDagilimiViewModel
            {
                Isletme = isletme,
                SayacTanimlari = sayactanimlari,
                IsletmeSayacDagilimi=new IsletmeSayacDagilimi()

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddIsletmeSayac(IsletmeSayacDagilimiViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("Isletme");
            ModelState.Remove("IsletmeSayacDagilimi.Sayac");
            ModelState.Remove("IsletmeSayacDagilimi.Isletme");
            ModelState.Remove("SayacTanimlari");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var isletmesayac = model.IsletmeSayacDagilimi;
                await _repository.AddAsync(isletmesayac, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "IsletmeSayacDagilimi");

        }
    }
}
