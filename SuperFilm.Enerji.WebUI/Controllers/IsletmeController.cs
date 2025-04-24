using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class IsletmeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public IsletmeController(ILogger<HomeController> logger,
            IRepository repository,
            IQueryRepository<EnerjiDbContext> queryRepository,
            IRepository<EnerjiDbContext> enerjiRepository)
        {
            _logger = logger;
            _repository = repository;
            _queryRepository = queryRepository;
            _enerjiRepository = enerjiRepository;
        }
        public async Task<IActionResult> Index(int id)
        {
            var query = _queryRepository.GetQueryable<Isletme>().Include(i => i.Isyeri);
            var Isletmeler = new List<Isletme>();
            if (id == 0)
            {
                Isletmeler = await query.ToListAsync();
            }
            else
            {
                Isletmeler = await query.Where(i => i.IsyeriId == id).ToListAsync();
            }

            var Isyerleri = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();
            var model = new IsYeriIsletmeListViewModel
            {
                Isletmeler = Isletmeler,
                IsYerleri = Isyerleri
            };
            return View(model);
        }
        public async Task<IActionResult> AddIsletme(int? id)
        {
            var isyerleri = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();
            Isletme isletmemodel = new Isletme();

            if (id != null)
            {
                var isletme = await _queryRepository
                    .GetQueryable<Isletme>()
                    .Include(i => i.Isyeri)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (isletme != null)
                {
                    isletmemodel = isletme;
                }
            }
            var model = new IsYeriIsletmeViewModel
            {
                Isletme = isletmemodel,
                IsYerleri = isyerleri
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddIsletme(IsYeriIsletmeViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("IsYerleri");
            ModelState.Remove("Isletme.Isyeri.Kodu");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                var isyerleri = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();
                Isletme isletmemodel = new Isletme();
                var m = new IsYeriIsletmeViewModel
                {
                    Isletme = isletmemodel,
                    IsYerleri = isyerleri
                };
                return View(m);
            }
            try
            {
                var isletme = model.Isletme;
                if(isletme.Id == 0)
                {
                    await _repository.AddAsync<Isletme>(isletme, cancellationToken);
                }
                else
                {
                    await _repository.UpdateAsync<Isletme>(isletme, cancellationToken);
                }

                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "Isletme");

        }
    }
}
