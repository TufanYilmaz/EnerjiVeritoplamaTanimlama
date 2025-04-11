using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class IsYeriController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public IsYeriController(ILogger<HomeController> logger,
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
            var query = _queryRepository.GetQueryable<IsYeri>().Include(i => i.Isletme);
            var IsYerleri = new List<IsYeri>();
            if (id == 0)
            {
                IsYerleri = await query.ToListAsync();
            }
            else {
                IsYerleri = await query.Where(i => i.IsletmeTanimlariId == id).ToListAsync();
            }
            
            var Isletmeler = await _queryRepository.GetQueryable<IsletmeTanimlari>().ToListAsync();
            var model = new IsYeriIsletmeListViewModel
            {
                IsYeri = IsYerleri,
                IsletmeTanimlari = Isletmeler
            };
            return View(model);
        }
        public async Task<IActionResult> AddIsYeri()
        {
            var isletmeler = await _queryRepository.GetQueryable<IsletmeTanimlari>().ToListAsync();
            var model = new IsYeriIsletmeViewModel
            {
                IsYeri = new IsYeri(),
                IsletmeTanimlari = isletmeler
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddIsYeri(IsYeriIsletmeViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("IsletmeTanimlari");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var isYeri = model.IsYeri;
                await _repository.AddAsync(isYeri, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "IsYeri");

        }
    }
}
