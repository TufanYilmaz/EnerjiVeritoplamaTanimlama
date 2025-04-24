
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class SayacVeriController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;

        public SayacVeriController(ILogger<HomeController> logger,
            IRepository repository,
            IQueryRepository<EnerjiDbContext> queryRepository,
            IRepository<EnerjiDbContext> enerjiRepository)
        {
            _logger = logger;
            _repository = repository;
            _queryRepository = queryRepository;
            _enerjiRepository = enerjiRepository;
        }
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> FilterSayacVeri()
        {
            
            var sayaclar = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            var opcnodeslar = await _queryRepository.GetQueryable<OpcNodes>().ToListAsync();

            var model = new RequestViewModel
            {
                SayacTanimlari = sayaclar,
                OpcNodes = opcnodeslar,
                SayacVeri = new List<SayacVeri>(),
                SayacVeriRequest = new SayacVeriRequest()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> FilterSayacVeri(SayacVeriRequest sayacVeriRequest)
        {
            var sayaclar = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            var opcnodeslar = await _queryRepository.GetQueryable<OpcNodes>().ToListAsync();
            List<SayacVeri> sayacverileri = [];

            if (ModelState.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                var m = new RequestViewModel
                {
                    SayacTanimlari = sayaclar,
                    OpcNodes = opcnodeslar,
                    SayacVeri = sayacverileri,
                    SayacVeriRequest = new SayacVeriRequest()
                };
                return View(m);
            }
            var sayacverileriQuery = _queryRepository.GetQueryable<SayacVeri>()
                .Where(r=> sayacVeriRequest.DataTypeId==1 ? r.SayacId>0 : r.OpcNodesId>0 );

            if (sayacVeriRequest.DataTypeId == 1 && sayacVeriRequest.SayacOpcNodesID > 0)
            {
                sayacverileriQuery= sayacverileriQuery.Where(i => i.SayacId == sayacVeriRequest.SayacOpcNodesID);
            }
            if (sayacVeriRequest.DataTypeId == 2 && sayacVeriRequest.SayacOpcNodesID > 0)
            {
                sayacverileriQuery = sayacverileriQuery.Where(i => i.OpcNodesId == sayacVeriRequest.SayacOpcNodesID);
            }
            if (sayacVeriRequest.StartDate !=null && sayacVeriRequest.StartDate != default)
            {
                sayacverileriQuery = sayacverileriQuery.Where(i => i.NormalizeDate > sayacVeriRequest.StartDate);
            }
            if (sayacVeriRequest.EndDate != null &&  sayacVeriRequest.EndDate != default)
            {
                sayacverileriQuery = sayacverileriQuery.Where(i => i.NormalizeDate < sayacVeriRequest.EndDate);
            }
            if (sayacVeriRequest.OrderBy == OrderBy.Ascending)
            {
                sayacverileriQuery = sayacverileriQuery.OrderBy(i => i.NormalizeDate);
            }
            if (sayacVeriRequest.OrderBy == OrderBy.Descending)
            {
                sayacverileriQuery = sayacverileriQuery.OrderByDescending(i => i.NormalizeDate);
            }

            sayacverileri = await sayacverileriQuery
                .Take(sayacVeriRequest.NumData)
                .ToListAsync();

            var model = new RequestViewModel
            {
                SayacTanimlari = sayaclar,
                OpcNodes = opcnodeslar,
                SayacVeri = sayacverileri,
                SayacVeriRequest = sayacVeriRequest
            };
            return View(model);
        }
        public IActionResult ListSayacVeri()
        {
            return View();
        }
    }
}
