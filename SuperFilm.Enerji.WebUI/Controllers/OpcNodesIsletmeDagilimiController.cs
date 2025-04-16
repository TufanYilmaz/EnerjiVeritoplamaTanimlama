using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class OpcNodesIsletmeDagilimiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public OpcNodesIsletmeDagilimiController(ILogger<HomeController> logger,
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
            var model = await _queryRepository.GetQueryable<OpcNodesIsletmeDagilimi>()
                                      .Include(isd => isd.OpcNodes)
                                      .Include(isd => isd.Isletme)
                                      .ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> AddOpcNodesIsletmeDagilimi()
        {
            var opcNodes = await _queryRepository.GetQueryable<OpcNodes>().ToListAsync();
            var ısletme = await _queryRepository.GetQueryable<Isletme>().ToListAsync();
            var model = new OpcNodesIsletmeDagilimiViewModel
            {
                OpcNodes = opcNodes,
                Isletme = ısletme,
                OpcNodesIsletmeDagilimi = new OpcNodesIsletmeDagilimi()

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddOpcNodesIsletmeDagilimi(OpcNodesIsletmeDagilimiViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("OpcNodes");
            ModelState.Remove("OpcNodesIsletmeDagilimi.Isletme");
            ModelState.Remove("OpcNodesIsletmeDagilimi.OpcNodes");
            ModelState.Remove("Isletme");

            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var opcnodesIsletme = model.OpcNodesIsletmeDagilimi;
                await _repository.AddAsync(opcnodesIsletme, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "OpcNodesIsletmeDagilimi");

        }
    }
}
