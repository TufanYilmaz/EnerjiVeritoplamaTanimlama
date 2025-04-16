using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class OpcNodesSayacDagilimiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public OpcNodesSayacDagilimiController(ILogger<HomeController> logger,
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
            var model = await _queryRepository.GetQueryable<OpcNodesSayacDagilimi>()
                                      .Include(isd => isd.OpcNodes)
                                      .Include(isd => isd.Sayac)
                                      .ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> AddOpcNodesSayacDagilimi()
        {
            var opcNodes = await _queryRepository.GetQueryable<OpcNodes>().ToListAsync();
            var sayactanimlari = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            var model = new OpcNodesSayacDagilimiViewModel
            {
                OpcNodes = opcNodes,
                SayacTanimlari = sayactanimlari,
                OpcNodesSayacDagilimi = new OpcNodesSayacDagilimi()

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddOpcNodesSayacDagilimi(OpcNodesSayacDagilimiViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("OpcNodes");
            ModelState.Remove("OpcNodesSayacDagilimi.Sayac");
            ModelState.Remove("OpcNodesSayacDagilimi.OpcNodes");
            ModelState.Remove("SayacTanimlari");

            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var opcnodesSayac = model.OpcNodesSayacDagilimi;
                await _repository.AddAsync(opcnodesSayac, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "OpcNodesSayacDagilimi");

        }
    }
}
