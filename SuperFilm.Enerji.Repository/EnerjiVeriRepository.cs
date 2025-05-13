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
    }
}
