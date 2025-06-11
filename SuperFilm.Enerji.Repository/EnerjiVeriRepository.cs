using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.Repository
{
    public class EnerjiVeriRepository<TDbContext> : IEnerjiVeriRepository<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        public EnerjiVeriRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SayacVeri>> GetDailyAsync(DateTime gun, int sayacId)
        {
            try
            {
                Console.WriteLine($"Günlük veri çekiliyor: {gun.Date}, SayacId: {sayacId}");
                
                // O gün için verileri direkt SQL ile sorgula (debug için log'la)
                var query = $"SELECT * FROM SAYAC_VERI WHERE CONVERT(date, NormalizeDate) = '{gun:yyyy-MM-dd}' AND SayacId = {sayacId} ORDER BY NormalizeDate";
                Console.WriteLine($"Sorgu: {query}");
                
                // O gün için verileri al - tam değerlerle çalışacağız
                var dailyData = await _dbContext.Set<SayacVeri>()
                    .Where(x => x.NormalizeDate.Date == gun.Date && x.SayacId == sayacId)
                    .OrderBy(x => x.NormalizeDate)
                    .ToListAsync();

                Console.WriteLine($"Toplam bulunan veri: {dailyData.Count}");
                
                if (dailyData == null || !dailyData.Any())
                {
                    Console.WriteLine("Veri bulunamadı!");
                    return new List<SayacVeri>();
                }

                // Her bir veriyi debug için logla
                foreach (var data in dailyData)
                {
                    Console.WriteLine($"Veri: Id={data.Id}, Zaman={data.Zaman}, Deger={data.Deger}, NormalizeDate={data.NormalizeDate}");
                }

                // 24 saat için sonuç listesi
                var result = new List<SayacVeri>();
                
                // Her saat için en son gelen veriyi al
                for (int hour = 0; hour < 24; hour++)
                {
                    string hourStr = hour.ToString("D2");
                    
                    // O saate ait veriler (ilk 2 karakteri eşleşenler)
                    var hourData = dailyData
                        .Where(x => x.Zaman != null && x.Zaman.Length >= 2 && x.Zaman.Substring(0, 2) == hourStr)
                        .OrderByDescending(x => x.NormalizeDate) // En son kaydı al
                        .FirstOrDefault();
                    
                    if (hourData != null)
                    {
                        Console.WriteLine($"Saat {hourStr} için veri bulundu: {hourData.Deger}");
                        
                        // Değeri doğrudan kullan, hiçbir dönüşüm/hesaplama yapma
                        result.Add(new SayacVeri
                        {
                            Id = hourData.Id,
                            Zaman = hourStr,  // Sadece saat bilgisi
                            Deger = hourData.Deger,  // Değeri olduğu gibi kullan
                            NormalizeDate = hourData.NormalizeDate,
                            SayacId = hourData.SayacId,
                            Kod = hourData.Kod
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Saat {hourStr} için veri bulunamadı.");
                    }
                }
                
                Console.WriteLine($"Toplam sonuç: {result.Count} saat için veri");
                return result.OrderBy(r => r.Zaman).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veri çekme hatası: {ex.Message}");
                return new List<SayacVeri>();
            }
        }

        public async Task<List<SayacVeri>> GetDailyDiffAsync(DateTime gun, int sayacId)
        {
            var daily = await _dbContext.Set<SayacVeri>().Where(x => x.NormalizeDate.Date == gun.Date && x.SayacId == sayacId)
              .GroupBy(x => x.NormalizeDate.Hour)
              .Select(r => r.OrderBy(r => r.NormalizeDate).FirstOrDefault())
              .ToListAsync();

            for (int i = 0; i < daily.Count - 1; i++)
            {
                daily[i].Deger = daily[i + 1].Deger - daily[i].Deger;
                daily[i].Zaman = daily[i].Zaman.Substring(0, 2);
            }

            daily.RemoveAt(daily.Count-1);
            return daily;
        }

        public async Task<List<SayacVeri>> GetMonthlyAsync(DateTime ay, int sayacId)
        {
            string yearStr = ay.ToString("yyyy");
            string monthStr = ay.ToString("MM");
            
            // O ay için verileri al
            var monthlyData = await _dbContext.Set<SayacVeri>()
                .Where(x => x.Yil == yearStr && x.Ay == monthStr && x.SayacId == sayacId)
                .OrderBy(x => x.NormalizeDate)
                .ToListAsync();

            if (monthlyData == null || !monthlyData.Any())
            {
                return new List<SayacVeri>();
            }

            // Ayın gün sayısını hesapla
            int daysInMonth = DateTime.DaysInMonth(ay.Year, ay.Month);
            
            // Tüm günler için veri listesi oluştur
            var result = new List<SayacVeri>();
            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayStr = day.ToString("D2");
                
                // Bu güne ait veri var mı?
                var dayData = monthlyData.Where(x => x.Gun == dayStr).OrderBy(x => x.NormalizeDate).FirstOrDefault();
                
                if (dayData != null)
                {
                    // Eğer gün için veri varsa ekle
                    result.Add(new SayacVeri 
                    { 
                        Gun = dayStr,  // Gün bilgisi (örn: "09")
                        Ay = monthStr, // Ay bilgisi (örn: "04")
                        Deger = dayData.Deger,
                        NormalizeDate = dayData.NormalizeDate,
                        SayacId = dayData.SayacId
                    });
                }
            }

            return result;
        }

        public async Task<List<SayacVeri>> GetMonthlyDiffAsync(DateTime ay, int sayacId)
        {
            var monthly = await _dbContext.Set<SayacVeri>().Where(x => x.Ay == ay.ToString("MM") && x.Yil == ay.ToString("yyyy") && x.SayacId == sayacId)
             .GroupBy(x => x.NormalizeDate.Day)
             .Select(r => r.OrderBy(r => r.NormalizeDate).FirstOrDefault())
             .ToListAsync();

            for (int i = 0; i < monthly.Count - 1; i++)
            {
                monthly[i].Deger = monthly[i + 1].Deger - monthly[i].Deger;
            }
            return monthly;
        }

        // Alternatif yöntem - Her gün için sadece son veriyi al (23:59 civarı)
        public async Task<List<SayacVeri>> GetMonthlyEndOfDayAsync(DateTime ay, int sayacId)
        {
            string yearStr = ay.ToString("yyyy");
            string monthStr = ay.ToString("MM");
            
            try
            {
                Console.WriteLine($"Aylık veri çekiliyor: {yearStr}-{monthStr}, SayacId: {sayacId}");
                
                // Ayın gün sayısını hesapla
                int daysInMonth = DateTime.DaysInMonth(ay.Year, ay.Month);
                var result = new List<SayacVeri>();
                
                // Her gün için veri oluştur
                for (int day = 1; day <= daysInMonth; day++)
                {
                    string dayStr = day.ToString("D2");
                    
                    // Günün son saatine (23:xx) ait veriyi bul (varsa)
                    var dayData = await _dbContext.Set<SayacVeri>()
                        .Where(x => x.Yil == yearStr && x.Ay == monthStr && x.Gun == dayStr && 
                               x.SayacId == sayacId && x.Zaman.StartsWith("23"))
                        .OrderByDescending(x => x.NormalizeDate)
                        .FirstOrDefaultAsync();
                    
                    // Eğer 23 saati yoksa, en son veriyi al
                    if (dayData == null)
                    {
                        dayData = await _dbContext.Set<SayacVeri>()
                            .Where(x => x.Yil == yearStr && x.Ay == monthStr && x.Gun == dayStr && x.SayacId == sayacId)
                            .OrderByDescending(x => x.NormalizeDate)
                            .FirstOrDefaultAsync();
                    }
                    
                    if (dayData != null)
                    {
                        Console.WriteLine($"Gün {dayStr} için veri bulundu: {dayData.Deger}");
                        
                        result.Add(new SayacVeri
                        {
                            Gun = dayStr,
                            Ay = monthStr,
                            Deger = dayData.Deger,
                            NormalizeDate = dayData.NormalizeDate,
                            SayacId = dayData.SayacId,
                            Kod = dayData.Kod
                        });
                    }
                    else 
                    {
                        Console.WriteLine($"Gün {dayStr} için veri bulunamadı.");
                    }
                }
                
                Console.WriteLine($"Toplam sonuç: {result.Count} gün için veri");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aylık veri çekme hatası: {ex.Message}");
                return new List<SayacVeri>();
            }
        }

        #region OpcNodes Methods

        public async Task<List<SayacVeri>> GetDistinctOpcNodeIds()
        {
            // SayacVeri içinde OpcNodesId != null ve SayacId == null (sadece OPC kayıtlar)
            return await _dbContext.Set<SayacVeri>()
                .Where(x => x.OpcNodesId != null && x.SayacId == null)
                .GroupBy(x => new { x.OpcNodesId, x.Kod })
                .Select(g => g.First())
                .ToListAsync();
        }



        public async Task<List<SayacVeri>> GetOpcNodeDailyAsync(DateTime gun, int opcNodeId)
        {
            try
            {
                Console.WriteLine($"OPC Node günlük veri çekiliyor: {gun.Date}, OpcNodeId: {opcNodeId}");
                
                // O gün için verileri direkt SQL ile sorgula (debug için log'la)
                var query = $"SELECT * FROM SAYAC_VERI WHERE CONVERT(date, NormalizeDate) = '{gun:yyyy-MM-dd}' AND OpcNodesId = {opcNodeId} AND SayacId IS NULL ORDER BY NormalizeDate";
                Console.WriteLine($"OPC Sorgu: {query}");
                
                // O gün için verileri al - tam değerlerle çalışacağız
                var dailyData = await _dbContext.Set<SayacVeri>()
                    .Where(x => x.NormalizeDate.Date == gun.Date && x.OpcNodesId == opcNodeId && x.SayacId == null)
                    .OrderBy(x => x.NormalizeDate)
                    .ToListAsync();

                Console.WriteLine($"Toplam bulunan OPC veri: {dailyData.Count}");
                
                if (dailyData == null || !dailyData.Any())
                {
                    Console.WriteLine("OPC veri bulunamadı!");
                    return new List<SayacVeri>();
                }

                // Her bir veriyi debug için logla
                foreach (var data in dailyData)
                {
                    Console.WriteLine($"OPC Veri: Id={data.Id}, Zaman={data.Zaman}, Deger={data.Deger}, NormalizeDate={data.NormalizeDate}");
                }

                // 24 saat için sonuç listesi
                var result = new List<SayacVeri>();
                
                // Her saat için en son gelen veriyi al
                for (int hour = 0; hour < 24; hour++)
                {
                    string hourStr = hour.ToString("D2");
                    
                    // O saate ait veriler (ilk 2 karakteri eşleşenler)
                    var hourData = dailyData
                        .Where(x => x.Zaman != null && x.Zaman.Length >= 2 && x.Zaman.Substring(0, 2) == hourStr)
                        .OrderByDescending(x => x.NormalizeDate) // En son kaydı al
                        .FirstOrDefault();
                    
                    if (hourData != null)
                    {
                        Console.WriteLine($"OPC Saat {hourStr} için veri bulundu: {hourData.Deger}");
                        
                        // Değeri doğrudan kullan, hiçbir dönüşüm/hesaplama yapma
                        result.Add(new SayacVeri
                        {
                            Id = hourData.Id,
                            Zaman = hourStr,  // Sadece saat bilgisi
                            Deger = hourData.Deger,  // Değeri olduğu gibi kullan
                            NormalizeDate = hourData.NormalizeDate,
                            OpcNodesId = hourData.OpcNodesId,
                            Kod = hourData.Kod
                        });
                    }
                    else
                    {
                        Console.WriteLine($"OPC Saat {hourStr} için veri bulunamadı.");
                    }
                }
                
                Console.WriteLine($"Toplam OPC sonuç: {result.Count} saat için veri");
                return result.OrderBy(r => r.Zaman).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OPC veri çekme hatası: {ex.Message}");
                return new List<SayacVeri>();
            }
        }

        public async Task<List<SayacVeri>> GetOpcNodeMonthlyEndOfDayAsync(DateTime ay, int opcNodeId)
        {
            string yearStr = ay.ToString("yyyy");
            string monthStr = ay.ToString("MM");
            
            try
            {
                Console.WriteLine($"Aylık OPC veri çekiliyor: {yearStr}-{monthStr}, OpcNodeId: {opcNodeId}");
                
                // Ayın gün sayısını hesapla
                int daysInMonth = DateTime.DaysInMonth(ay.Year, ay.Month);
                var result = new List<SayacVeri>();
                
                // Her gün için veri oluştur
                for (int day = 1; day <= daysInMonth; day++)
                {
                    string dayStr = day.ToString("D2");
                    
                    // Günün son saatine (23:xx) ait veriyi bul (varsa)
                    var dayData = await _dbContext.Set<SayacVeri>()
                        .Where(x => x.Yil == yearStr && x.Ay == monthStr && x.Gun == dayStr && 
                               x.OpcNodesId == opcNodeId && x.SayacId == null && x.Zaman.StartsWith("23"))
                        .OrderByDescending(x => x.NormalizeDate)
                        .FirstOrDefaultAsync();
                    
                    // Eğer 23 saati yoksa, en son veriyi al
                    if (dayData == null)
                    {
                        dayData = await _dbContext.Set<SayacVeri>()
                            .Where(x => x.Yil == yearStr && x.Ay == monthStr && x.Gun == dayStr && 
                                   x.OpcNodesId == opcNodeId && x.SayacId == null)
                            .OrderByDescending(x => x.NormalizeDate)
                            .FirstOrDefaultAsync();
                    }
                    
                    if (dayData != null)
                    {
                        Console.WriteLine($"OPC Gün {dayStr} için veri bulundu: {dayData.Deger}");
                        
                        result.Add(new SayacVeri
                        {
                            Gun = dayStr,
                            Ay = monthStr,
                            Deger = dayData.Deger,
                            NormalizeDate = dayData.NormalizeDate,
                            OpcNodesId = dayData.OpcNodesId,
                            Kod = dayData.Kod
                        });
                    }
                    else 
                    {
                        Console.WriteLine($"OPC Gün {dayStr} için veri bulunamadı.");
                    }
                }
                
                Console.WriteLine($"Toplam OPC sonuç: {result.Count} gün için veri");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aylık OPC veri çekme hatası: {ex.Message}");
                return new List<SayacVeri>();
            }
        }

        #endregion

        public async Task<(decimal minValue, decimal maxValue, decimal interval)> CalculateChartAxisValues(List<SayacVeri> data)
        {
            decimal minValue = 0;
            decimal maxValue = 0;
            
            if (data != null && data.Any())
            {
                // 0 olmayan değerlerin min/max'ını bul
                var nonZeroValues = data.Where(c => c.Deger != 0).Select(c => c.Deger).ToList();
                if (nonZeroValues.Any())
                {
                    minValue = nonZeroValues.Min();
                    maxValue = nonZeroValues.Max();
                }
                else
                {
                    minValue = data.Min(c => c.Deger);
                    maxValue = data.Max(c => c.Deger);
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
                    // Aralığın %10 altında ve üstünde değerlerle göster
                    minValue = minValue - (range * 0.1m);
                    maxValue = maxValue + (range * 0.1m);
                }
                
                // 0'ın altındaysa 0'dan başlat
                if (minValue > 0)
                    minValue = 0;
            }

            // Y ekseni için uygun aralık hesapla
            decimal interval = CalculateYAxisInterval(minValue, maxValue);
            
            return (minValue, maxValue, interval);
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

        public List<SayacVeri> CompleteDailyData(List<SayacVeri> existingData)
        {
            var result = new List<SayacVeri>();
            
            // Her saat için kontrol et
            for (int hour = 0; hour < 24; hour++)
            {
                string hourStr = $"{hour:D2}";
                
                // Bu saat için veri var mı?
                var hourData = existingData.FirstOrDefault(d => d.Zaman == hourStr);
                
                if (hourData != null)
                {
                    // Mevcut veriyi ekle
                    result.Add(hourData);
                }
                else
                {
                    // Eksik saat için 0 değeri ekle
                    result.Add(new SayacVeri
                    {
                        Zaman = hourStr,
                        Deger = 0
                    });
                }
            }
            
            return result.OrderBy(r => r.Zaman).ToList();
        }

        public List<SayacVeri> CompleteMonthlyData(List<SayacVeri> existingData, DateTime selectedMonth)
        {
            var result = new List<SayacVeri>();
            
            // Ayın gün sayısını hesapla
            int daysInMonth = DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month);
            string monthStr = selectedMonth.ToString("MM");
            
            // Her gün için kontrol et
            for (int day = 1; day <= daysInMonth; day++)
            {
                string dayStr = $"{day:D2}";
                
                // Bu gün için veri var mı?
                var dayData = existingData.FirstOrDefault(d => d.Gun == dayStr);
                
                if (dayData != null)
                {
                    // Mevcut veriyi ekle
                    result.Add(dayData);
                }
                else
                {
                    // Eksik gün için 0 değeri ekle
                    result.Add(new SayacVeri
                    {
                        Gun = dayStr,
                        Ay = monthStr,
                        Deger = 0
                    });
                }
            }
            
            return result.OrderBy(r => int.Parse(r.Gun)).ToList();
        }

        public async Task<List<SayacVeri>> OpcGetDailyDiffAsync(DateTime gun, int? OpcNodesId)
        {
            List<SayacVeri> result = new List<SayacVeri>();
            var daily = await _dbContext.Set<SayacVeri>().Where(x => x.NormalizeDate.Date == gun.Date && x.OpcNodesId == OpcNodesId)
               .GroupBy(x => x.NormalizeDate.Hour)
               .Select(r => r.OrderBy(r => r.NormalizeDate).FirstOrDefault())
               .ToListAsync();
            var nextFirstValue = await _dbContext.Set<SayacVeri>()
                .Where(x => x.NormalizeDate.Date == gun.Date.AddDays(1) && x.OpcNodesId == OpcNodesId)
                .OrderBy(r => r.NormalizeDate)
                .Take(1)
                .FirstOrDefaultAsync();
            bool sonDegerVar = false;
            if(nextFirstValue!= null || nextFirstValue != default)
            {
                sonDegerVar = true;
                daily.Add(nextFirstValue);
            }
            for (int i = 0; i < daily.Count - 1; i++)
            {
                result.Add(new SayacVeri()
                {
                    Deger = daily[i + 1].Deger - daily[i].Deger,
                    Zaman = daily[i].Zaman.Substring(0, 2) + "-" + daily[i + 1].Zaman.Substring(0, 2)
                });
            }
            return result;
        }

        public async Task<List<SayacVeri>> OpcGetMonthlyDiffAsync(DateTime ay, int? OpcNodesId)
        {
            List<SayacVeri> result = new List<SayacVeri>();
            var monthly = await _dbContext.Set<SayacVeri>()
                .Where(x => x.Ay == ay.ToString("MM") && x.Yil == ay.ToString("yyyy") && x.OpcNodesId == OpcNodesId)
             .GroupBy(x => x.NormalizeDate.Day)
             .Select(r => r.OrderBy(r => r.NormalizeDate).FirstOrDefault())
             .ToListAsync();
            var nextFirstValue = await _dbContext.Set<SayacVeri>()
                .Where(x => x.NormalizeDate.Date == ay.Date.AddMonths(1) && x.OpcNodesId == OpcNodesId)
                .OrderBy(r => r.NormalizeDate)
                .Take(1)
                .FirstOrDefaultAsync();
            bool sonDegerVar = false;
            if (nextFirstValue != null || nextFirstValue != default)
            {
                sonDegerVar = true;
                monthly.Add(nextFirstValue);
            }
            for (int i = 0; i < monthly.Count - 1; i++)
            {
                result.Add(new SayacVeri()
                {
                    Deger = monthly[i + 1].Deger - monthly[i].Deger,
                    Gun= monthly[i].Gun,
                    Ay= monthly[i].Ay,
                });
            }
            return result;
        }
    }
}

