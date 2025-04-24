using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.WebUI.ViewModels;
using TanvirArjel.EFCore.GenericRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Dynamic.Core;

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



    }
}