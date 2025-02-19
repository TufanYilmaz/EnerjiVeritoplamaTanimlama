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
        private readonly IQueryRepository _queryRepository;
        private readonly EnerjiDbContext _context;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public IsletmeController(ILogger<HomeController> logger,
            IRepository repository,
            IQueryRepository queryRepository,
            EnerjiDbContext context,
            IRepository<EnerjiDbContext> enerjiRepository)
        {
            _logger = logger;
            _repository = repository;
            _context = context;
            _queryRepository = queryRepository;
            _enerjiRepository = enerjiRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model =await _queryRepository.GetQueryable<IsletmeTanimlari>().ToListAsync();
            return View(model);
        }
    }
}
