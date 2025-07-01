using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SapWebServices.Helpers;
using SapWebServices.Model;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.Entites.Migrations;
using System.Globalization;
using System.Runtime.CompilerServices;
using TanvirArjel.EFCore.GenericRepository;

namespace SapWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnerjiController : ControllerBase
    {
        private readonly ILogger<EnerjiController> _logger;
		private readonly IDataAccess _dataAccess;
		private readonly IDB2Helper _db2Helper;
        private readonly IRepository _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly IRepository<EnerjiDbContext> _enerjiRepository;
        private readonly int TOLERANCE_AS02 = 3;
        private readonly int TOLERANCE_AS01 = 10;

        public EnerjiController(ILogger<EnerjiController> logger,
            IDataAccess dataAccess,
            IDB2Helper dB2Helper,
            IRepository repository,
            IRepository<EnerjiDbContext> enerjiRepository,
            IQueryRepository<EnerjiDbContext> queryRepository)
        {
            _logger = logger;
            _dataAccess = dataAccess;
            _db2Helper = dB2Helper;
            _repository = repository;
            _enerjiRepository = enerjiRepository;
            _queryRepository = queryRepository;
        }
        //[HttpGet(Name = "GetDirect")]
        //public EnerjiModel Get(string uretimYeri , DateTime startDateTime,DateTime endDateTime)
        //{
        //    return DB2Helper.GetEnerji(uretimYeri, startDateTime,endDateTime,null);
        //}
        [HttpPost(Name = "Get")]
        public EnerjiModel Get(EnerjiRequestModel enerjiRequest)
        {
            return _db2Helper.GetEnerji(enerjiRequest);
        }
        [HttpPost("GetEnerjiList", Name = "GetList")]
        //[Route("api/[controller]/GetEnerji")]
        public List<EnerjiModelDetail> GetEnerji(EnerjiRequestModelList enerjiRequest)
        {
            List<EnerjiModelDetail> res = new List<EnerjiModelDetail>();
            foreach (var enerji in enerjiRequest.EnerjiModel ?? new List<EnerjiRequestSimpleModel>())
            {
                var tobeAdded =new EnerjiModelDetail(_db2Helper.GetEnerji(
                        new EnerjiRequestModel()
                        {
                            HelperPlants = enerjiRequest.HelperPlants,
                            StartDateTime = enerji.StartDateTime,
                            EndDateTime = enerji.EndDateTime,
                            ProductionLine = enerji.ProductionLine
                        }
                    ));
                tobeAdded.StartDateTime = enerji.StartDateTime.ToString();
                tobeAdded.EndDateTime = enerji.EndDateTime.ToString();
                res.Add(tobeAdded);
            }
            return res;
        }
        [HttpPost("GetEnerjiAdvanceList", Name = "GetAdvanceList")]
        //[Route("api/[controller]/GetEnerji")]
        public async Task<List<EnerjiModelAdvanceDetail>> GetEnerjiAdvece(EnerjiRequestAdvaceModelList enerjiRequest)
        {

            #region Gelen İstek kaydet
            //var dbRes =_dataAccess.GetAsycn();
            EnerjiRequest enerjiRequestEntity = new EnerjiRequest();
            enerjiRequestEntity.EnerjiRequestAdvanceBody = new List<EnerjiRequestAdvance>();
            foreach (var enerji in enerjiRequest.EnerjiModel)
            {
                enerjiRequestEntity.EnerjiRequestAdvanceBody.Add(new EnerjiRequestAdvance()
                {
                    ProductionLine = enerji.ProductionLine,
                    StartDate = enerji.StartDate,
                    StartTime = enerji.StartTime,
                    EndDate = enerji.EndDate,
                    EndTime = enerji.EndTime,
                });

            }
            try
            {
                await _enerjiRepository.AddAsync<EnerjiRequest>(enerjiRequestEntity);
                await _enerjiRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            } 
            #endregion

            List<EnerjiModelAdvanceDetail> res = new List<EnerjiModelAdvanceDetail>();

            if (enerjiRequest.EnerjiModel.Count == 0)
            {
                return res;
            }
            var isletmeKod = enerjiRequest.EnerjiModel.First().ProductionLine;
            var isletme = await _queryRepository.GetAsync<Isletme>(r => r.Kod == isletmeKod, r => r.Include(x => x.Isyeri));
            if (isletme != null && isletme.Isyeri!.Kodu == "AS02")
            {

                var OpcNodesIsletmeDagilimi = await _queryRepository
                    .GetListAsync<OpcNodesIsletmeDagilimi>(r => r.IsletmeId == isletme.Id,includes:r=>r.Include(x=>x.OpcNodes));
                var notAcceptedRequest = enerjiRequest.EnerjiModel.Where(r =>
                        r.StartDate == null
                        || r.StartTime == null
                        || r.EndDate == null
                        || r.EndTime == null
                        || r.StartDate == "NULL"
                        || r.StartTime == "NULL"
                        || r.EndDate == "NULL"
                        || r.EndTime == "NULL"
                        ).ToList();
                foreach (var item in notAcceptedRequest)
                {
                    string startTime = string.Empty, endTime = string.Empty;
                    if (
                        (item.StartTime==null && item.EndTime==null) 
                        //|| (item.StartTime.ToString().ToLower()=="null" && item.EndTime.ToString().ToLower() == "null")
                        )
                    {
                        startTime = "000000";
                        endTime = "000000";
                    }
                    else if (item.StartTime==null 
                        //|| item.StartTime.ToString().ToLower() == "null"
                        )
                    {
                        DateTime saat = DateTime.ParseExact(item.EndTime, "HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        startTime = saat.AddMinutes(-5).ToString("HHmmss");
                        endTime = item.EndTime;

                    }
                    else if (item.EndTime==null 
                        //|| item.EndTime.ToString().ToLower() == "null"
                        )
                    {
                        DateTime saat = DateTime.ParseExact(item.StartTime, "HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        endTime = saat.AddMinutes(5).ToString("HHmmss");
                        startTime = item.StartTime;
                    }
                    res.Add(new EnerjiModelAdvanceDetail()
                    {
                        StartDate = item.StartDate,
                        StartTime = startTime,
                        EndDate = item.EndDate,
                        EndTime = endTime,
                        DDeger = 0,
                        EDeger = 0,
                        UretimYeri = item.ProductionLine
                    });
                }
                var acceptedRequest = enerjiRequest.EnerjiModel
                    .Where(r =>
                        r.StartDate != null
                        && r.StartTime != null
                        && r.EndDate != null
                        && r.EndTime != null
                        && r.StartDate != "NULL"
                        && r.StartTime != "NULL"
                        && r.EndDate != "NULL"
                        && r.EndTime != "NULL"
                        ).ToList();
                var simpleRequest = acceptedRequest
                    .Select(r =>
                    new EnerjiRequestSimpleModel()
                    {
                        StartDateTime = DateTime.ParseExact(r.StartDate + r.StartTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                        EndDateTime = DateTime.ParseExact(r.EndDate + r.EndTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)
                    })
                    .ToList();
                decimal deger = 0;
                List<int> sayacIds = OpcNodesIsletmeDagilimi.Select(id => id.OpcNodesId).ToList();
                DateTime globalMin = simpleRequest.Min(x => x.StartDateTime.AddMinutes(TOLERANCE_AS02));
                DateTime globalMax = simpleRequest.Max(x => x.EndDateTime.AddMinutes(TOLERANCE_AS02));
                var sayacVerileri = await _queryRepository
                    .GetListAsync<SayacVeri>(r => r.NormalizeDate > globalMin
                    && r.NormalizeDate < globalMax
                    && r.OpcNodesId != null
                    && sayacIds.Contains(r.OpcNodesId.Value)
                    );
                List<TargetResult> targetResults = new List<TargetResult>();
                foreach (var opdNodeIsletme in OpcNodesIsletmeDagilimi)
                {
                    var targetResultFromRequest = simpleRequest
                    .Select(target => new TargetResult()
                    {
                        Target = target,
                        StartMatch = sayacVerileri.Where(r => r.OpcNodesId == opdNodeIsletme.OpcNodesId)
                            .Where(p => Math.Abs((p.NormalizeDate - target.StartDateTime).TotalMinutes) <= TOLERANCE_AS02)
                            .OrderBy(p => Math.Abs((p.NormalizeDate - target.StartDateTime).TotalMinutes))
                            .FirstOrDefault(),
                        EndMatch = sayacVerileri.Where(r => r.OpcNodesId == opdNodeIsletme.OpcNodesId)
                            .Where(p => Math.Abs((p.NormalizeDate - target.EndDateTime).TotalMinutes) <= TOLERANCE_AS02)
                            .OrderBy(p => Math.Abs((p.NormalizeDate - target.EndDateTime).TotalMinutes))
                            .FirstOrDefault(),
                        OpcNodesIsletmeD = opdNodeIsletme,
                    })
                    .Where(x => x.StartMatch != null && x.EndMatch != null)
                    .ToList();
                    targetResults.AddRange(targetResultFromRequest);
                }
                var toaddResults = targetResults
                    .Where(r=>
                    r.StartMatch!=null 
                    && r.EndMatch!=null)
                    .GroupBy(r => r.Target)
                    .Select(g => new EnerjiModelAdvanceDetail()
                    {
                        StartDate = g.First().Target.StartDateTime.ToString("yyyyMMdd"),
                        StartTime = g.First().Target.StartDateTime.ToString("HHmmss"),
                        EndDate = g.First().Target.EndDateTime.ToString("yyyyMMdd"),
                        EndTime = g.First().Target.EndDateTime.ToString("HHmmss"),
                        DDeger= (double)g.Sum(r=>(r.EndMatch.Deger-r.StartMatch.Deger)/1000),
                        EDeger =(double)g.Where(r=>r.OpcNodesIsletmeD.Carpan<1)
                            .Sum(r=> r.EndMatch.Deger*r.OpcNodesIsletmeD.Carpan -
                            r.StartMatch.Deger*r.OpcNodesIsletmeD.Carpan)/1000,
                            //0,//g.First().OpcNodesIsletmeD. // (double)g.Sum(r => (r.EndMatch.Deger - r.StartMatch.Deger) / 1000),
                        UretimYeri= enerjiRequest.EnerjiModel.First().ProductionLine,
                    });
                res.AddRange(toaddResults);
                try
                {
                    var responseModel = new List<EnerjiResponseAdvance>();
                    foreach (var item in res)
                    {
                        responseModel.Add(new EnerjiResponseAdvance()
                        {
                            DDeger=item.DDeger,
                            EDeger=item.EDeger,
                            EndDate=item.EndDate,
                            EndTime=item.EndTime,
                            StartDate=item.StartDate,
                            StartTime=item.StartTime,
                            UretimYeri= enerjiRequest.EnerjiModel.First().ProductionLine,
                            EnerjiRequestId= enerjiRequestEntity.Id
                        });
                    }
                    await _enerjiRepository.AddAsync<EnerjiResponseAdvance>(responseModel);
                    await _enerjiRepository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {


                #region AS01
                foreach (var enerji in enerjiRequest.EnerjiModel ?? new List<EnerjiRequestAdvanceModel>())
                {
                    try
                    {
                        var exist = _dataAccess.GetAsycn(enerji);

                        if (exist.Result != null)
                        {
                            res.Add(new EnerjiModelAdvanceDetail(exist.Result));
                            continue;
                        }
                        DateTime startDate;
                        DateTime endDate;
                        if (DateTime.TryParseExact(enerji.StartDate + enerji.StartTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out startDate)
                            &&
                                   DateTime.TryParseExact(enerji.EndDate + enerji.EndTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out endDate))
                        {
                            var tobeAdded = new EnerjiModelAdvanceDetail(_db2Helper.GetEnerji(
                              new EnerjiRequestModel()
                              {
                                  HelperPlants = enerjiRequest.HelperPlants,
                                  StartDateTime = startDate,
                                  EndDateTime = endDate,
                                  ProductionLine = enerji.ProductionLine
                              }
                          ));
                            tobeAdded.StartDate = startDate.ToString("yyyyMMdd");
                            tobeAdded.StartTime = startDate.ToString("HHmmss");
                            tobeAdded.EndDate = endDate.ToString("yyyyMMdd");
                            tobeAdded.EndTime = endDate.ToString("HHmmss");
                            res.Add(tobeAdded);
                            if (tobeAdded.EDeger > 0 || tobeAdded.DDeger > 0)
                            {
                                var r = _dataAccess.Add(new EnerjiRequestAdvanceModelDb(tobeAdded));
                            }
                        }
                        else
                        {
                        }

                    }
                    catch (Exception exx)
                    {
                        continue;
                    }
                }
            }
            #endregion
            return res;
        }

    }

    public class TargetResult
    {
        public TargetResult()
        {
        }

        public EnerjiRequestSimpleModel Target { get; set; }
        public SayacVeri StartMatch { get; set; }
        public SayacVeri EndMatch { get; set; }
        public OpcNodesIsletmeDagilimi OpcNodesIsletmeD { get; set; }
    }
}
