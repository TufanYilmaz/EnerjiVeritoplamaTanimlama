using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
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
        public async Task<IActionResult> Index()
        {
            var model =await _queryRepository.GetQueryable<IsletmeTanimlari>().ToListAsync();
            return View(model);
        }
        public IActionResult AddIsletme()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddIsletme(IsletmeTanimlari model,CancellationToken cancellationToken)
        {
            if(ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                await _repository.AddAsync(model, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "Isletme");
        }
        public async Task<IActionResult> ListIsYeri()
        {
            var model = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();
            
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
            return RedirectToAction("ListIsYeri", "Isletme");
        }
        public async Task<IActionResult> GetIsletme(int id, CancellationToken cancellationToken)
        {
            var model = await _enerjiRepository.GetByIdAsync<IsletmeTanimlari>(id,asNoTracking:true,cancellationToken: cancellationToken);
            return View(model);
        }
    }
}
