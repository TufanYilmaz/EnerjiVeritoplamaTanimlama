using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;
using Microsoft.AspNetCore.Authorization;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index()
        {
            var model =await _queryRepository.GetQueryable<IsYeri>().ToListAsync();
            return View(model);
        }
        public IActionResult AddIsYeri()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddIsYeri(IsYeri model,CancellationToken cancellationToken)
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

            return RedirectToAction("Index", "IsYeri");
        }
        
    }
}
