using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SapWebServices.Helpers;
using SapWebServices.Model;
using System.Globalization;

namespace SapWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnerjiController : ControllerBase
    {
        private readonly ILogger<EnerjiController> _logger;
		private readonly IDataAccess _dataAccess;
		private readonly IDB2Helper _db2Helper;
        public EnerjiController(ILogger<EnerjiController> logger,IDataAccess dataAccess,IDB2Helper dB2Helper)
        {
            _logger = logger;
            _dataAccess = dataAccess;
            _db2Helper = dB2Helper;
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
        public List<EnerjiModelAdvanceDetail> GetEnerjiAdvece(EnerjiRequestAdvaceModelList enerjiRequest)
        {
            //var dbRes =_dataAccess.GetAsycn();
            List<EnerjiModelAdvanceDetail> res = new List<EnerjiModelAdvanceDetail>();
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
                        if(tobeAdded.EDeger>0 || tobeAdded.DDeger > 0)
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
            return res;
        }

    }
}
