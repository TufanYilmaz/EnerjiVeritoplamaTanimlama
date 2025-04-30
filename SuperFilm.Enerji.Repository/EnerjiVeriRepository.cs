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
           var daily = await _dbContext.Set<SayacVeri>().Where(x => x.NormalizeDate.Date == gun.Date && x.SayacId == sayacId)
                .GroupBy(x => x.NormalizeDate.Hour)
                .Select(r=>r.OrderBy(r => r.NormalizeDate).FirstOrDefault())
                .ToListAsync();

            for (int i = 0; i < daily.Count-1; i++)
            {
                daily[i].Deger = daily[i + 1].Deger - daily[i].Deger;
                daily[i].Zaman = daily[i].Zaman.Substring(0, 2);
            }

            return daily;
        }

        public async Task<List<SayacVeri>> GetMonthlyAsync(DateTime ay, int sayacId)
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
    }

}
