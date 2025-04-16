using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class OpcNodesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public OpcNodesController(ILogger<HomeController> logger,
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
            var model = await _queryRepository.GetQueryable<OpcNodes>().ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> AddOpcNodesAsync()
        {
            var isletmeler = await _queryRepository.GetQueryable<Isletme>().ToListAsync();
            var model = new OpcNodesIsletmeViewModel

            {
                OpcNodes = new OpcNodes(),
                Isletmeler = isletmeler
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOpcNodes(OpcNodesIsletmeViewModel model, CancellationToken cancellationToken)

        {
            ModelState.Remove("Isletmeler");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var opcNodes = model.OpcNodes;
                await _repository.AddAsync(opcNodes, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "OpcNodes");
        }
    }
}