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
            var model = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> AddSayac()
        {
            var isletmeler = await _queryRepository.GetQueryable<IsletmeTanimlari>().ToListAsync();
            var isyerleri = await _queryRepository.GetQueryable<IsYeri>().ToListAsync();
            var model = new SayacViewModel
            {
                SayacTanimlari = new SayacTanimlari(),
                IsletmeTanimlari = isletmeler,
                IsYeri = isyerleri
            };
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> AddSayac(SayacViewModel model, CancellationToken cancellationToken)
        {
            ModelState.Remove("IsletmeTanimlari");
            ModelState.Remove("IsYeri");
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return NoContent();
            }
            try
            {
                var sayac = model.SayacTanimlari;
                await _repository.AddAsync(sayac, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction("Index", "Sayac");
        }
        public async Task<IActionResult> GetSayac(int id, CancellationToken cancellationToken)
        {
            var model = await _enerjiRepository.GetByIdAsync<SayacTanimlari>(id, asNoTracking: true, cancellationToken: cancellationToken);
            return View(model);
        }   
    }
}
