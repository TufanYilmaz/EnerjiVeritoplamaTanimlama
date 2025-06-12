using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;
using Microsoft.AspNetCore.Authorization;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Manager")]
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
            var opcnodesisletmeler = await _queryRepository.GetQueryable<OpcNodesIsletmeDagilimi>().Include(osd => osd.OpcNodes).Include(osd => osd.Isletme).ToListAsync();
            var isyerleri = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();

            var model = new OpcNodesIsletmeIsYeriListViewModel
            {
                OpcNodesIsletmeDagilimi = opcnodesisletmeler,
                IsYeri = isyerleri
            };

            return View(model);
        }
        public async Task<IActionResult> AddOpcNodesIsletme(int? id)
        {
            var opcNodes = await _queryRepository.GetQueryable<OpcNodes>().ToListAsync();
            var isletmeler = await _queryRepository.GetQueryable<Isletme>().ToListAsync();
            var isyeri = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();

            OpcNodesIsletmeDagilimi opcnodesisletmemodel = new OpcNodesIsletmeDagilimi();
            if (id != null)
            {
                var opcnodesisletme = await _queryRepository
                    .GetQueryable<OpcNodesIsletmeDagilimi>()
                    .Include(i => i.OpcNodes)
                    .Include(i => i.Isletme)
                    .FirstOrDefaultAsync(x => x.Id == id);
           
                if (opcnodesisletme != null)
                {
                    opcnodesisletmemodel = opcnodesisletme;
                }
            }
            var model = new OpcNodesIsletmeDagilimiViewModel
            {
                OpcNodes = opcNodes,
                Isletme = isletmeler,
                OpcNodesIsletmeDagilimi = opcnodesisletmemodel,
                IsYeri = isyeri
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddOpcNodesIsletme(OpcNodesIsletmeDagilimiViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("Isletme");
            ModelState.Remove("OpcNodes");
            ModelState.Remove("OpcNodesIsletmeDagilimi.Isletme");
            ModelState.Remove("OpcNodesIsletmeDagilimi.OpcNodes");


            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var opcnodesisletme = model.OpcNodesIsletmeDagilimi;
                if(opcnodesisletme.Id == 0)
                {
                    await _repository.AddAsync<OpcNodesIsletmeDagilimi>(opcnodesisletme, cancellationToken);
                }
                else
                {
                    await _repository.UpdateAsync<OpcNodesIsletmeDagilimi>(opcnodesisletme, cancellationToken);
                }

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
