using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.Models;
using System.Diagnostics;
using TanvirArjel.EFCore.GenericRepository;
using Microsoft.AspNetCore.Authorization;

namespace SuperFilm.Enerji.WebUI.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository _queryRepository;
        private readonly EnerjiDbContext _context;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public HomeController(ILogger<HomeController> logger,
            IRepository repository,
			IQueryRepository<EnerjiDbContext> queryRepository,
			EnerjiDbContext context,
			IRepository<EnerjiDbContext> enerjiRepository)
		{
			_logger = logger;
            _repository = repository;
            _context = context;
            _queryRepository = queryRepository;
            _enerjiRepository = enerjiRepository;
        }

		public IActionResult Index()
		{
            return View();
		}

		public IActionResult Privacy()
		{
            return View();
		}
        public IActionResult Karpuz()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
