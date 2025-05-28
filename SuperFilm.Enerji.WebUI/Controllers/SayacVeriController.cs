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
using System;
using Microsoft.AspNetCore.Authorization;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    [Authorize]
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
                    data=data.Where(e=>e.Kod.Contains(searchValue) || e.OpcNodesId.ToString().Contains(searchValue) || e.Deger.ToString().Contains(searchValue) || e.NormalizeDate.ToString().Contains(searchValue));
                }
                int totalRecord = data.Count();
                var cData=data.Skip(skip).Take(pageSize).ToList();

                var list = new List<SayacVeriList>();
                //var currentSayaclar = await _queryRepository.GetQueryable<SayacTanimlari>().Where(x => cData.All(r=>r.SayacId==x.Id)).ToListAsync();
                //var currentOpcNodelar = _queryRepository.GetQueryable<OpcNodes>().FirstOrDefault(x => x.Id == c.OpcNodesId);

                if (cData.Count != 0)
                {
                    var query = from v in cData.AsEnumerable()
                                join s in _queryRepository.GetQueryable<SayacTanimlari>().AsEnumerable()
                                    on v.SayacId equals s.Id into sayacGroup
                                from s in sayacGroup.DefaultIfEmpty()

                                join o in _queryRepository.GetQueryable<OpcNodes>().AsEnumerable()
                                    on v.OpcNodesId equals o.Id into opcGroup
                                from o in opcGroup.DefaultIfEmpty()

                                select new SayacVeriList()
                                {
                                    Kod = v.Kod,
                                    SayacTanimi = s != null ? s.SayacTanimi : null,
                                    Description = o != null ? o.Description : null,
                                    Deger = v.Deger,
                                    NormalizeDate = v.NormalizeDate,
                                };
                    list = query.ToList();
                }

                //if (cData.Count != 0)
                //{
                //    foreach (var c in cData)
                //    {
                //        var kod = "";
                //        var sayacTanim = "";
                //        var description = "";

                //        var sayacquery = _queryRepository.GetQueryable<SayacTanimlari>().FirstOrDefault(x => x.SayacKodu == c.Kod);
                //        var opcnodequery = _queryRepository.GetQueryable<OpcNodes>().FirstOrDefault(x => x.Id == c.OpcNodesId);

                //        // Sayaca aitse
                //        if (c.Kod != null && sayacquery != null)
                //        {
                //            kod = c.Kod;
                //            sayacTanim = sayacquery.SayacTanimi;
                //            description = null;
                //        }
                //        // OpcNodes aitse
                //        if (c.Kod != null && opcnodequery != null)
                //        {
                //            kod = opcnodequery.Code;
                //            sayacTanim = null;
                //            description = opcnodequery.Description;
                //        }

                //        list.Add(new SayacVeriList
                //        {
                //            Kod = kod,
                //            SayacTanimi = sayacTanim,
                //            Description = description,
                //            NormalizeDate = c.NormalizeDate,
                //            Deger = c.Deger,

                //        });
                //    }
                //}

                var jsonData = new
                {
                    draw = draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = list
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
                var query = from v in sayacverileriQuery.Take(sayacVeriRequest.NumData).AsEnumerable()
                            join s in sayaclar.AsEnumerable()
                                on v.SayacId equals s.Id into sayacGroup
                            from s in sayacGroup.DefaultIfEmpty()

                            join o in opcnodeslar.AsEnumerable()
                                on v.OpcNodesId equals o.Id into opcGroup
                            from o in opcGroup.DefaultIfEmpty()
                            
                            select new SayacVeriList()
                            {
                                Kod = v.Kod,
                                SayacTanimi = s != null ? s.SayacTanimi : null,
                                Description = o != null ? o.Description : null,
                                Deger = v.Deger,
                                NormalizeDate = v.NormalizeDate,
                            };


                // sayacverileri = await sayacverileriQuery
                //.Take(sayacVeriRequest.NumData)
                //.ToListAsync();

                // var list = new List<SayacVeriList>();

                //                if (sayacverileri.Count != 0)
                //                {
                //                    foreach (var c in sayacverileri)
                //                    {
                //                        var kod = "";
                //                        var sayacTanim = "";
                //                        var description = "";
                //                        /*
                //select S.SayacTanimi,O.Description, V.* from SAYAC_VERI V
                //left join SAYAC_TANIMLARI S on S.Id=V.SayacId
                //left join OPC_NODES O on O.Id=V.OpcNodesId 
                //where OpcNodesId>0 or SayacId<12
                //                         */
                //                        var sayacquery = _queryRepository.GetQueryable<SayacTanimlari>().FirstOrDefault(x => x.SayacKodu == c.Kod);
                //                        var opcnodequery = _queryRepository.GetQueryable<OpcNodes>().FirstOrDefault(x => x.Id == c.OpcNodesId);

                //                        // Sayaca aitse
                //                        if (c.Kod != null && sayacquery != null)
                //                        {
                //                            kod = c.Kod;
                //                            sayacTanim = sayacquery.SayacTanimi;
                //                            description = null;
                //                        }
                //                        // OpcNodes aitse
                //                        if (c.Kod == null && opcnodequery != null)
                //                        {
                //                            kod = opcnodequery.Code;
                //                            sayacTanim = null;
                //                            description = opcnodequery.Description;
                //                        }

                //                        list.Add(new SayacVeriList
                //                        {
                //                            Kod = kod,
                //                            SayacTanimi = sayacTanim,
                //                            Description = description,
                //                            NormalizeDate = c.NormalizeDate,
                //                            Deger = c.Deger,

                //                        });
                //                    }
                //                }
                var list = query.ToList();

                int totalRecord = list.Count();

                var jsonData = new
                {
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = list,
                };
                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
