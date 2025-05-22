using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;
using Microsoft.AspNetCore.Authorization;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    [Authorize]
    public class IsletmeSayacDagilimiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public IsletmeSayacDagilimiController(ILogger<HomeController> logger,
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
            var model = await _queryRepository.GetQueryable<IsletmeSayacDagilimi>()
                                      .Include(isd => isd.Isletme)
                                      .Include(isd => isd.Sayac)
                                      .ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> AddIsletmeSayac(int? id)
        {
            var isletmeler = await _queryRepository.GetQueryable<Isletme>().ToListAsync();
            var sayactanimlari = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            IsletmeSayacDagilimi isletmesayacmodel = new IsletmeSayacDagilimi();

            if (id != null)
            {
                var isletmesayac = await _queryRepository
                    .GetQueryable<IsletmeSayacDagilimi>()
                    .Include(i => i.Isletme)
                    .Include(i => i.Sayac)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (isletmesayac != null)
                {
                    isletmesayacmodel = isletmesayac;
                }
            }
            var model = new IsletmeSayacDagilimiViewModel
            {
                Isletme = isletmeler,
                SayacTanimlari = sayactanimlari,
                IsletmeSayacDagilimi = isletmesayacmodel

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddIsletmeSayac(IsletmeSayacDagilimiViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("Isletme");
            ModelState.Remove("IsletmeSayacDagilimi.Sayac");
            ModelState.Remove("IsletmeSayacDagilimi.Isletme");
            ModelState.Remove("SayacTanimlari");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var isletmesayac = model.IsletmeSayacDagilimi;
                if (isletmesayac.Id == 0)
                {
                    await _repository.AddAsync<IsletmeSayacDagilimi>(isletmesayac, cancellationToken);
                }
                else
                {
                    await _repository.UpdateAsync<IsletmeSayacDagilimi>(isletmesayac, cancellationToken);
                }
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "IsletmeSayacDagilimi");

        }
    }
}
