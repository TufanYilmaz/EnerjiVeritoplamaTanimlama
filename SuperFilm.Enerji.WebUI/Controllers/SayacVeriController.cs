
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Dynamic.Core;
using Hangfire.Storage;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

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

        public async Task<IActionResult> Listele() 
        {
            //var data = await _queryRepository.GetQueryable<SayacVeri>().ToListAsync();
            //return View(data);
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
                SayacVeriRequest = new SayacVeriRequest()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetData()
        {
            try
            {
                int pageSize = 0;
                var draw = Request.Form["draw"].FirstOrDefault();
                var start=Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns["+Request.Form["order[0][column]" ].FirstOrDefault()+"][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                pageSize= length != null?Convert.ToInt32(length) : 0;
                int skip=start!=null ? Convert.ToInt32(start) : 0;
                var data = _queryRepository.GetQueryable<SayacVeri>();
                if(!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
                {
                    data = data.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data=data.Where(e=>e.Kod.Contains(searchValue)||/* e.SayacId.ToString().Contains(searchValue)||*/ e.OpcNodesId.ToString().Contains(searchValue)|| e.NormalizeDate.ToString().Contains(searchValue));
                }
                int totalRecord = data.Count();
                var cData=data.Skip(skip).Take(pageSize).ToList();
                var jsonData = new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = cData
                };
                return new JsonResult(jsonData);

            }
            catch (Exception)
            {

                throw;
            }
            
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
                    SayacVeriRequest = new SayacVeriRequest()

                };
                return View(m);
            }
            var sayacverileriQuery = _queryRepository.GetQueryable<SayacVeri>()
                .Where(r => sayacVeriRequest.DataTypeId == 1 ? r.SayacId > 0 : r.OpcNodesId > 0);

            if (sayacVeriRequest.DataTypeId == 1 && sayacVeriRequest.SayacID > 0)
            {
                sayacverileriQuery = sayacverileriQuery.Where(i => i.SayacId == sayacVeriRequest.SayacID);
            }
            if (sayacVeriRequest.DataTypeId == 2 && sayacVeriRequest.OpcNodesID > 0)
            {
                sayacverileriQuery = sayacverileriQuery.Where(i => i.OpcNodesId == sayacVeriRequest.OpcNodesID);
            }
            if (sayacVeriRequest.StartDate != null && sayacVeriRequest.StartDate != default)
            {
                sayacverileriQuery = sayacverileriQuery.Where(i => i.NormalizeDate > sayacVeriRequest.StartDate);
            }
            if (sayacVeriRequest.EndDate != null && sayacVeriRequest.EndDate != default)
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
  
            try
            {
                sayacverileri = await sayacverileriQuery
               .Take(sayacVeriRequest.NumData)
               .ToListAsync();
                int totalRecord = sayacverileri.Count();

                var jsonData = new
                {
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = sayacverileri
                };
                return new JsonResult(jsonData);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
