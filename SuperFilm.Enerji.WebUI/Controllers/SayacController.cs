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
            return View();
        }
     
        public IActionResult AddSayac()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSayac(SayacTanimlari model, CancellationToken cancellationToken)
        {
            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
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
            return Ok();
        }
        public async Task<IActionResult> GetSayac(int id, CancellationToken cancellationToken)
        {
            var model = await _enerjiRepository.GetByIdAsync<SayacTanimlari>(id, asNoTracking: true, cancellationToken: cancellationToken);
            return View(model);
        }
    }
}
