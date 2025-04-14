using System.Threading;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class SayacController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public SayacController(ILogger<HomeController> logger,
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
            var model = await _queryRepository.GetQueryable<SayacTanimlari>().Include(r=>r.Isyeri).Where(s=>s.IsDeleted==0).ToListAsync();
            return View(model);

        }
        public async Task<IActionResult> AddSayac(int? id)
        {
            var isletmeler = await _queryRepository.GetQueryable<IsletmeTanimlari>().ToListAsync();
            var isyerleri = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();
            SayacTanimlari sayacModel = new SayacTanimlari();
            IsletmeTanimlari isletmeTanimi = new IsletmeTanimlari();
            if (id != null)
            {
                var sayac = await _queryRepository
                    .GetQueryable<SayacTanimlari>()
                    .Include(r => r.Isyeri)
                    .ThenInclude(r => r.Isletme)
                    .FirstOrDefaultAsync(x => x.Id == id);

                isletmeTanimi = await _queryRepository
                    .GetQueryable<IsletmeTanimlari>()
                    .FirstOrDefaultAsync(x => x.Id == sayac.Isyeri.Isletme.Id);

                if (sayac != null)
                {
                    sayacModel=sayac;
                }
            }
            var model = new SayacViewModel
            {
                SayacTanimlari = sayacModel,
                IsletmeTanimi = isletmeTanimi,
                IsletmeTanimlari = isletmeler,
                IsYeri = isyerleri,

            };
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> AddSayac(SayacViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("IsletmeTanimi.IsletmeKodu");
            ModelState.Remove("IsletmeTanimlari");
            ModelState.Remove("IsYeri");
            ModelState.Remove("SayacTanimlari.Isyeri");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var sayac = model.SayacTanimlari;
                if (sayac.Id == 0)
                {
                    await _repository.AddAsync<SayacTanimlari>(sayac, cancellationToken);

                }
                else
                {
                    await _repository.UpdateAsync<SayacTanimlari>(sayac, cancellationToken);
                }
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "Sayac");
        }
        public async Task<IActionResult> DeleteSayac(int? id, CancellationToken cancellationToken)
        {
            if (id != null)
            {
                var sayac = await _queryRepository
                    .GetQueryable<SayacTanimlari>()
                    .Include(r => r.Isyeri)
                    .ThenInclude(r => r.Isletme)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (sayac != null)
                {
                    sayac.IsDeleted = 1;
                    await _repository.UpdateAsync<SayacTanimlari>(sayac, cancellationToken);
                    await _repository.SaveChangesAsync(cancellationToken);
                }
            }

            return RedirectToAction("Index", "Sayac");
        }
    }
}
