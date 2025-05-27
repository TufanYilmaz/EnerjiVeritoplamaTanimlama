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
            var availableNodeIds = await _repository.GetDistinctOpcNodeIds();
            return View(availableNodeIds);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(int TimeTypeId, DateTime? Gun, DateTime? Ay, int? OpcNodesId, int? OpcNodesId2)
        {
            List<LineChartData> chartData = new List<LineChartData>();
            List<LineChartData> chartData2 = new List<LineChartData>();
            
            try
            {
                if (!OpcNodesId.HasValue || OpcNodesId <= 0)
                {
                    ViewBag.ErrorMessage = "Geçerli bir OPC Node ID seçilmedi.";
                    var errorNodeIds = await _repository.GetDistinctOpcNodeIds();
                    return View(errorNodeIds);
                }

                List<SayacVeri> data = null;
                List<SayacVeri> data2 = null;
                
                if (TimeTypeId == 1 && Gun.HasValue) // Günlük
                {
                    _logger?.LogInformation($"Günlük OPC veri çekiliyor: Tarih {Gun.Value:dd/MM/yyyy}, OpcNodeId: {OpcNodesId}");

                    data = await _repository.GetOpcNodeDailyAsync(Gun.Value, OpcNodesId.Value);
                    if (data != null && data.Any())
                    {
                        data = _repository.CompleteDailyData(data);
                        ViewBag.ChartTitle = $"OPC Node - {OpcNodesId} - Günlük Veriler ({Gun.Value:dd/MM/yyyy})";
                    }
                    chartData = _repository.OpcGetDailyDiffAsync(Gun.Value, OpcNodesId).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();

                    if (OpcNodesId2.HasValue && OpcNodesId2 > 0)
                    {
                        data2 = await _repository.GetOpcNodeDailyAsync(Gun.Value, OpcNodesId2.Value);
                        if (data2 != null && data2.Any())
                        {
                            
                            data2 = _repository.CompleteDailyData(data2);
                            ViewBag.ChartTitle = $"OPC Node Karşılaştırma - Günlük Veriler ({Gun.Value:dd/MM/yyyy})";
                        }
                        chartData2 = _repository.OpcGetDailyDiffAsync(Gun.Value, OpcNodesId2).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();
                    }
                }
                else if (TimeTypeId == 2 && Ay.HasValue) // Aylık
                {
                    _logger?.LogInformation($"Aylık OPC veri çekiliyor: Ay {Ay.Value:MM/yyyy}, OpcNodeId: {OpcNodesId}");

                    data = await _repository.GetOpcNodeMonthlyEndOfDayAsync(Ay.Value, OpcNodesId.Value);
                    if (data != null && data.Any())
                    {
                        data = _repository.CompleteMonthlyData(data, Ay.Value);
                        ViewBag.ChartTitle = $"OPC Node - {OpcNodesId} - Aylık Veriler ({Ay.Value:MM/yyyy})";
                    }
                    chartData = _repository.OpcGetMonthlyDiffAsync(Ay.Value, OpcNodesId).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();

                    if (OpcNodesId2.HasValue && OpcNodesId2 > 0)
                    {
                        data2 = await _repository.GetOpcNodeMonthlyEndOfDayAsync(Ay.Value, OpcNodesId2.Value);
                        if (data2 != null && data2.Any())
                        {
                            data2 = _repository.CompleteMonthlyData(data2, Ay.Value);
                            ViewBag.ChartTitle = $"OPC Node Karşılaştırma - Aylık Veriler ({Ay.Value:MM/yyyy})";
                        }
                        chartData2 = _repository.OpcGetMonthlyDiffAsync(Ay.Value, OpcNodesId2).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();
                    }
                }

                if ((data != null && data.Any()) || (data2 != null && data2.Any()))
                {
                    var allValues = new List<decimal>();
                    if (chartData.Any()) allValues.AddRange(chartData.Select(d => d.Deger));
                    if (chartData2.Any()) allValues.AddRange(chartData2.Select(d => d.Deger));

                    var (minValue, maxValue) = (0, allValues.Max());
                    ViewBag.MinValue = (int)minValue;
                    ViewBag.MaxValue = (int)(maxValue * (decimal)1.05);
                    ViewBag.Interval = (int)(maxValue - minValue) / Math.Max(chartData.Count, chartData2.Count);
                }
                else
                {
                    _logger?.LogWarning($"OpcNodeId {OpcNodesId} için veri bulunamadı.");
                    chartData = new List<LineChartData>();
                    chartData2 = new List<LineChartData>();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "OPC veri çekerken hata oluştu");
                ViewBag.ErrorMessage = "OPC veri çekme işlemi sırasında bir hata oluştu: " + ex.Message;
            }

            // ViewData ve ViewBag'i güncelle
            ViewData["ChartData"] = chartData;
            ViewData["ChartData2"] = chartData2;
            ViewBag.HasData = chartData.Count > 0 || chartData2.Count > 0;
            ViewBag.TimeTypeId = TimeTypeId;
            ViewBag.SelectedOpcNodeId = OpcNodesId;
            ViewBag.SelectedOpcNodeId2 = OpcNodesId2;
            
            // JSON serialization ayarlarıyla kültüre uygun ondalık gösterimi
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Culture = CultureInfo.InvariantCulture
            };
            
            ViewBag.ChartDataJson = JsonConvert.SerializeObject(chartData, jsonSettings);
            ViewBag.ChartData2Json = JsonConvert.SerializeObject(chartData2, jsonSettings);
            
            // OpcNodes tablosundan ID'leri tekrar çek
            var finalNodeIds = await _repository.GetDistinctOpcNodeIds();
            return View(finalNodeIds);
        }

        public class LineChartData
        {
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

                List<SayacVeri> data = null;
                
                if (TimeTypeId == 1 && Gun.HasValue) // Günlük
                {
                    _logger?.LogInformation($"Günlük OPC veri çekiliyor: Tarih {Gun.Value:dd/MM/yyyy}, OpcNodeId: {OpcNodesId}");

                    data = await _repository.GetOpcNodeDailyAsync(Gun.Value, OpcNodesId.Value);
                    if (data != null && data.Any())
                    {
                        data = _repository.CompleteDailyData(data);
                        chartTitle = $"OPC Node - {OpcNodesId} - Günlük Veriler ({Gun.Value:dd/MM/yyyy})";
                    }
                    chartData = _repository.OpcGetDailyDiffAsync(Gun.Value, OpcNodesId).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();
                }
                else if (TimeTypeId == 2 && Ay.HasValue) // Aylık
                {
                    _logger?.LogInformation($"Aylık OPC veri çekiliyor: Ay {Ay.Value:MM/yyyy}, OpcNodeId: {OpcNodesId}");

                    data = await _repository.GetOpcNodeMonthlyEndOfDayAsync(Ay.Value, OpcNodesId.Value);
                    if (data != null && data.Any())
                    {
                        data = _repository.CompleteMonthlyData(data, Ay.Value);
                        chartTitle = $"OPC Node - {OpcNodesId} - Aylık Veriler ({Ay.Value:MM/yyyy})";
                    }
                    chartData = _repository.OpcGetMonthlyDiffAsync(Ay.Value, OpcNodesId).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();
                }

                if (data != null && data.Any())
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
