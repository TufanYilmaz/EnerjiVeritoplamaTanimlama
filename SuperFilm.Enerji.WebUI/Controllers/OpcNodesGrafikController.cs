using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.Repository;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
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
        public async Task<IActionResult> Index(int TimeTypeId, DateTime? Gun, DateTime? Ay, int? OpcNodesId)
        {
            List<LineChartData> chartData = new List<LineChartData>();
            
            try
            {
                if (!OpcNodesId.HasValue || OpcNodesId <= 0)
                {
                    ViewBag.ErrorMessage = "Geçerli bir OPC Node ID seçilmedi.";
                    var errorNodeIds = await _repository.GetDistinctOpcNodeIds();
                    return View(errorNodeIds);
                }

                List<SayacVeri> data = null;
                
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
                }

                if (data != null && data.Any())
                {
                    // Verileri LineChartData'ya dönüştür
                    //chartData = chartData.Select(d => new LineChartData
                    //{
                    //    Zaman = TimeTypeId == 1 ? $"{d.Zaman}:00" : $"{d.Gun}/{d.Ay}",
                    //    Deger = d.Deger
                    //}).ToList();

                    // Grafik eksen değerlerini hesapla
                    //var (minValue, maxValue, interval) = await _repository.CalculateChartAxisValues(data);
                    var (minValue, maxValue) = (0, chartData.Max(r => r.Deger));
                    ViewBag.MinValue = (int)minValue;
                    ViewBag.MaxValue = (int)(maxValue*(decimal)1.05);
                    ViewBag.Interval =(int)( maxValue-minValue)/chartData.Count;
                }
                else
                {
                    _logger?.LogWarning($"OpcNodeId {OpcNodesId} için veri bulunamadı.");
                    chartData = new List<LineChartData>();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "OPC veri çekerken hata oluştu");
                ViewBag.ErrorMessage = "OPC veri çekme işlemi sırasında bir hata oluştu: " + ex.Message;
            }

            // ViewData ve ViewBag'i güncelle
            ViewData["ChartData"] = chartData;
            ViewBag.HasData = chartData.Count > 0;
            ViewBag.TimeTypeId = TimeTypeId;
            ViewBag.SelectedOpcNodeId = OpcNodesId;
            
            // JSON serialization ayarlarıyla kültüre uygun ondalık gösterimi
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Culture = CultureInfo.InvariantCulture
            };
            
            ViewBag.ChartDataJson = JsonConvert.SerializeObject(chartData, jsonSettings);
            
            // OpcNodes tablosundan ID'leri tekrar çek
            var finalNodeIds = await _repository.GetDistinctOpcNodeIds();
            return View(finalNodeIds);
        }

        public class LineChartData
        {
            public string Zaman { get; set; }
            public decimal Deger { get; set; }
        }
    }
}
