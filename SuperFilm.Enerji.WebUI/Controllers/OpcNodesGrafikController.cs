using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.Repository;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    [Authorize]
    public class OpcNodesGrafikController : Controller
    {
        private readonly EnerjiVeriRepository<EnerjiDbContext> _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly ILogger<OpcNodesGrafikController> _logger;

        public OpcNodesGrafikController(
            EnerjiVeriRepository<EnerjiDbContext> repository,
            IQueryRepository<EnerjiDbContext> queryRepository,
            ILogger<OpcNodesGrafikController> logger = null
        )
        {
            _repository = repository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var opcTanimlari = await _queryRepository.GetQueryable<OpcNodes>().ToListAsync();
            return View(opcTanimlari);
        }
        
        

        public class LineChartData
        {
            public string Gun { get; set; }
            public string Zaman { get; set; }
            public decimal Deger { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> GetGraphData(int TimeTypeId, DateTime? Gun, DateTime? Ay, int? OpcNodesId)
        {
            List<LineChartData> chartData = new List<LineChartData>();
            string chartTitle = "";
            
            
            try
            {
                if (!OpcNodesId.HasValue || OpcNodesId <= 0)
                {
                    return Json(new { success = false, message = "Geçerli bir OPC Node ID seçilmedi." });
                }
                if (TimeTypeId == 1 && Gun.HasValue) // Günlük
                {
                    _logger?.LogInformation($"Günlük OPC veri çekiliyor: Tarih {Gun.Value:dd/MM/yyyy}, OpcNodeId: {OpcNodesId}");
                    chartData = _repository
                        .OpcGetDailyDiffAsync(Gun.Value, OpcNodesId)
                        .Result
                        .Select(r => new LineChartData() { 
                            Deger = r.Deger, 
                            Zaman = r.Zaman,
                            Gun = r.Gun,
                        }).ToList();
                }
                else if (TimeTypeId == 2 && Ay.HasValue) // Aylık
                {
                    _logger?.LogInformation($"Aylık OPC veri çekiliyor: Ay {Ay.Value:MM/yyyy}, OpcNodeId: {OpcNodesId}");
                    chartData = _repository
                        .OpcGetMonthlyDiffAsync(Ay.Value, OpcNodesId)
                        .Result
                        .Select(r => new LineChartData() { 
                            Deger = r.Deger, 
                            Zaman = r.Zaman,
                            Gun = r.Gun,
                        }).ToList();
                }

                if (chartData != null && chartData.Any())
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.None,
                        Culture = CultureInfo.InvariantCulture
                    };
                    return Json(new
                    {
                        success = true,
                        chartData = JsonConvert.SerializeObject(chartData, jsonSettings),
                        chartTitle = chartTitle,
                        selectedNodeId = OpcNodesId
                    });
                }
                else
                {
                    _logger?.LogWarning($"OpcNodeId {OpcNodesId} için veri bulunamadı.");
                    return Json(new { success = false, message = "Seçilen kriterlere uygun veri bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "OPC veri çekerken hata oluştu");
                return Json(new { success = false, message = "OPC veri çekme işlemi sırasında bir hata oluştu: " + ex.Message });
            }
        }
    }
}
