using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.Repository;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Controllers
{
    public class SayacVeriGrafikController : Controller
    {
        private readonly EnerjiVeriRepository<EnerjiDbContext> _repository;
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;
        private readonly ILogger<SayacVeriGrafikController> _logger;

        public SayacVeriGrafikController(
            EnerjiVeriRepository<EnerjiDbContext> repository,
            IQueryRepository<EnerjiDbContext> queryRepository,
            ILogger<SayacVeriGrafikController> logger = null
        )
        {
            _repository = repository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var sayactanımları = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            return View(sayactanımları);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(int TimeTypeId, DateTime? Gun, DateTime? Ay, int SayacId)
        {
            List<LineChartData> chartData = new List<LineChartData>();
            
            try
            {
                if (TimeTypeId == 1 && Gun.HasValue) // Günlük
                {
                    _logger?.LogInformation($"Günlük veri çekiliyor: Tarih {Gun.Value:dd/MM/yyyy}, SayacId: {SayacId}");

                    // Repository'den verileri çek
                    var days = await _repository.GetDailyAsync(Gun.Value, SayacId);

                    if (days != null && days.Any())
                    {
                        // Debuglama için tüm verileri yazdır
                        _logger?.LogInformation($"Veritabanından {days.Count} kayıt alındı");
                        foreach (var day in days)
                        {
                            _logger?.LogInformation($"Saat: {day.Zaman}, Değer: {day.Deger}");
                        }

                        // Repository'den gelen verileri doğrudan LineChartData'ya dönüştür
                        chartData = days.Select(d => new LineChartData
                        {
                            Zaman = $"{d.Zaman}:00",  // Saat formatı: HH:00
                            Deger = d.Deger,          // Değeri olduğu gibi al
                        }).ToList();

                        // Verilerin tam olduğundan emin ol (eksik saatler için 0 ekle)
                        chartData = CompleteDailyData(chartData);

                        ViewBag.ChartTitle = "Günlük Veriler - " + Gun.Value.ToString("dd/MM/yyyy");
                        _logger?.LogInformation($"Toplam {chartData.Count} günlük veri oluşturuldu");

                        // Debug için değerleri logla
                        foreach (var item in chartData)
                        {
                            _logger?.LogDebug($"Grafik verisi: Zaman={item.Zaman}, Değer={item.Deger}");
                        }
                    }
                    else
                    {
                        _logger?.LogWarning($"SayacId {SayacId} için {Gun.Value:dd/MM/yyyy} tarihine ait veri bulunamadı.");
                        chartData = GenerateSampleDailyData(Gun.Value);
                    }
                    chartData = _repository.GetDailyDiffAsync(Gun.Value, SayacId).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();
                }
                else if (TimeTypeId == 2 && Ay.HasValue) // Aylık
                {
                    _logger?.LogInformation($"Aylık veri çekiliyor: Ay {Ay.Value:MM/yyyy}, SayacId: {SayacId}");

                    // Performans iyileştirmesi: Her gün için 23:59 verisini al
                    var months = await _repository.GetMonthlyEndOfDayAsync(Ay.Value, SayacId);

                    if (months != null && months.Any())
                    {
                        // Repository'den gelen verileri doğrudan kullan
                        chartData = months.Select(m => new LineChartData
                        {
                            Zaman = $"{m.Gun}/{m.Ay}", // Gün/Ay formatı
                            Deger = m.Deger
                        }).ToList();

                        // Eksik günler için tamamlayıcı veri oluştur
                        chartData = CompleteMonthlyData(chartData, Ay.Value);

                        ViewBag.ChartTitle = "Aylık Veriler - " + Ay.Value.ToString("MM/yyyy");
                        _logger?.LogInformation($"Toplam {chartData.Count} aylık veri bulundu.");
                    }
                    else
                    {
                        _logger?.LogWarning($"SayacId {SayacId} için {Ay.Value:MM/yyyy} ayına ait veri bulunamadı.");
                        chartData = GenerateSampleMonthlyData(Ay.Value);
                    }
                    chartData = _repository.GetMonthlyDiffAsync(Ay.Value, SayacId).Result.Select(r => new LineChartData() { Deger = r.Deger, Zaman = r.Zaman }).ToList();

                }

                // Verileri sırala
                if (TimeTypeId == 1)
                {
                    // Saate göre sırala (00:00, 01:00, ...)
                    chartData = chartData.OrderBy(c => {
                        if (int.TryParse(c.Zaman.Split(':')[0], out int hour))
                            return hour;
                        return 0;
                    }).ToList();
                }
                else if (TimeTypeId == 2)
                {
                    // Güne göre sırala (01/04, 02/04, ...)
                    chartData = chartData.OrderBy(c => {
                        if (int.TryParse(c.Zaman.Split('/')[0], out int day))
                            return day;
                        return 0;
                    }).ToList();
                }

                // Y ekseni için min/max değerlerini hesapla
                decimal minValue = 0;
                decimal maxValue = 0;
                
                if (chartData.Any())
                {
                    // 0 olmayan değerlerin min/max'ını bul
                    var nonZeroValues = chartData.Where(c => c.Deger != 0).Select(c => c.Deger).ToList();
                    if (nonZeroValues.Any())
                    {
                        minValue = nonZeroValues.Min();
                        maxValue = nonZeroValues.Max();
                    }
                    else
                    {
                        minValue = chartData.Min(c => c.Deger);
                        maxValue = chartData.Max(c => c.Deger);
                    }
                    
                    // Uygun Y ekseni aralığı hesapla
                    var range = maxValue - minValue;
                    
                    // Eğer tüm değerler aynıysa veya aralık çok küçükse, aralık oluştur
                    if (range < 1)
                    {
                        minValue = minValue * 0.9m;
                        maxValue = maxValue * 1.1m;
                    }
                    else
                    {
                        // Aralığın %10 altında ve üstünde değerlerle göster (daha okunaklı grafik için)
                        minValue = minValue - (range * 0.1m);
                        maxValue = maxValue + (range * 0.1m);
                    }
                    
                    // 0'ın altındaysa 0'dan başlat
                    if (minValue > 0)
                        minValue = 0;
                }

                // ViewBag ile grafik ayarlarını aktar 
                ViewBag.MinValue = (int)minValue;
                ViewBag.MaxValue = (int)maxValue;
                
                // Uygun değer aralığı hesapla
                decimal interval = CalculateYAxisInterval(minValue, maxValue);
                ViewBag.Interval = interval;
                
                _logger?.LogInformation($"Grafik aralıkları: Min={minValue}, Max={maxValue}, Interval={interval}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Sayaç veri çekerken hata oluştu");
                ViewBag.ErrorMessage = "Veri çekme işlemi sırasında bir hata oluştu: " + ex.Message;
                
                // Hata durumunda örnek veri göster
                //if (TimeTypeId == 1 && Gun.HasValue)
                //{
                //    chartData = GenerateSampleDailyData(Gun.Value);
                //}
                //else if (TimeTypeId == 2 && Ay.HasValue)
                //{
                //    chartData = GenerateSampleMonthlyData(Ay.Value);
                //}
            }

            // ViewData ve ViewBag'i güncelle
            ViewData["ChartData"] = chartData;
            ViewBag.HasData = chartData.Count > 0;
            ViewBag.TimeTypeId = TimeTypeId;
            ViewBag.ShowZeroValues = true; // 0 değerleri grafikte göster/gizle
            
            // JSON serialization ayarlarıyla kültüre uygun ondalık gösterimi
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Culture = CultureInfo.InvariantCulture
            };
            
            ViewBag.ChartDataJson = JsonConvert.SerializeObject(chartData, jsonSettings);

            var sayactanımları = await _queryRepository.GetQueryable<SayacTanimlari>().ToListAsync();
            return View(sayactanımları);
        }

        private decimal CalculateYAxisInterval(decimal min, decimal max)
        {
            // Y ekseni için uygun aralık hesapla
            decimal range = max - min;
            
            if (range <= 0)
                return 1;
                
            // Uygun basamak ve aralık belirle
            int exponent = (int)Math.Floor(Math.Log10((double)range));
            decimal magnitude = (decimal)Math.Pow(10, exponent);
            
            // Genellikle 5-10 aralık olsun
            decimal normalized = range / magnitude;
            
            if (normalized < 2.5m)
                return magnitude / 5;
            else if (normalized < 5m)
                return magnitude / 2;
            else if (normalized < 10m)
                return magnitude;
            else
                return magnitude * 2;
        }

        private List<LineChartData> CompleteDailyData(List<LineChartData> existingData)
        {
            var completeData = new List<LineChartData>();
            
            // Her saat için kontrol et
            for (int hour = 0; hour < 24; hour++)
            {
                string hourKey = $"{hour:D2}:00";
                
                // Bu saat için veri var mı kontrol et
                var existingHour = existingData.FirstOrDefault(d => d.Zaman == hourKey);
                
                if (existingHour != null)
                {
                    // Mevcut veriyi ekle
                    completeData.Add(existingHour);
                    _logger?.LogDebug($"Saat {hourKey} için veri var: {existingHour.Deger}");
                }
                else
                {
                    // Eksik saat için null değerli veri ekle (grafikte boşluk olması için)
                    completeData.Add(new LineChartData 
                    { 
                        Zaman = hourKey, 
                        Deger = 0 // Grafikte boşluk göstermek istersek null kullanılabilir
                    });
                    _logger?.LogDebug($"Saat {hourKey} için veri yok, 0 eklendi");
                }
            }
            
            return completeData;
        }
        
        private List<LineChartData> CompleteMonthlyData(List<LineChartData> existingData, DateTime selectedMonth)
        {
            var completeData = new List<LineChartData>();
            
            // Ayın gün sayısını al
            int daysInMonth = DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month);
            
            // Her gün için kontrol et
            for (int day = 1; day <= daysInMonth; day++)
            {
                string dayKey = $"{day:D2}/{selectedMonth.Month:D2}";
                
                // Bu gün için veri var mı kontrol et
                var existingDay = existingData.FirstOrDefault(d => d.Zaman == dayKey);
                
                if (existingDay != null)
                {
                    completeData.Add(existingDay); // Mevcut veriyi ekle
                }
                else
                {
                    // Eksik gün için null değerli veri ekle
                    completeData.Add(new LineChartData 
                    { 
                        Zaman = dayKey, 
                        Deger = 0 // Grafikte boşluk göstermek istersek null kullanılabilir
                    });
                }
            }
            
            return completeData;
        }
        
        private List<LineChartData> GenerateSampleDailyData(DateTime selectedDay)
        {
            var chartData = new List<LineChartData>();
            Random random = new Random();
            
            // 24 saat için örnek veri oluştur
            for (int hour = 0; hour < 24; hour++)
            {
                chartData.Add(new LineChartData
                {
                    Zaman = $"{hour:D2}:00",
                    Deger = random.Next(50, 500)
                });
            }

            return chartData;
        }
        
        private List<LineChartData> GenerateSampleMonthlyData(DateTime selectedMonth)
        {
            var chartData = new List<LineChartData>();
            Random random = new Random();
            
            // Ayın gün sayısını al
            int daysInMonth = DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month);
            
            // Her gün için örnek veri ekle
            for (int day = 1; day <= daysInMonth; day++)
            {
                chartData.Add(new LineChartData
                {
                    Zaman = $"{day:D2}/{selectedMonth.Month:D2}",
                    Deger = random.Next(200, 1000)
                });
            }

            return chartData;
        }
        
        public class LineChartData
        {
            public string Zaman { get; set; }
            public decimal Deger { get; set; }
        }

       
    }
}
