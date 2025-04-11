using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class SayacController(
        ILogger<HomeController> _logger,
        IRepository _repository,
        IQueryRepository<EnerjiDbContext> _queryRepository,
        IRepository<EnerjiDbContext> _enerjiRepository) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await _queryRepository.GetQueryable<SayacTanimlari>().Include(r=>r.Isyeri).ToListAsync();
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
    }
}
