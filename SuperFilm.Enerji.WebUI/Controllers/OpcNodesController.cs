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
        public async Task<IActionResult> AddOpcNodesAsync(int? id)
        {

            OpcNodes opcnodesmodel = new OpcNodes();
            if(id != null)
            {
                var opcnodes = await _queryRepository
                    .GetQueryable<OpcNodes>()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (opcnodes != null)
                {
                    opcnodesmodel = opcnodes;
                }
            }

            return View(opcnodesmodel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOpcNodes(OpcNodes opcnodes, CancellationToken cancellationToken)

        {
            ModelState.Remove("Isletmeler");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {

                if(opcnodes.Id == 0)
                {
                    await _repository.AddAsync<OpcNodes>(opcnodes, cancellationToken);
                }
                else
                {
                    await _repository.UpdateAsync<OpcNodes>(opcnodes, cancellationToken);
                }
                
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