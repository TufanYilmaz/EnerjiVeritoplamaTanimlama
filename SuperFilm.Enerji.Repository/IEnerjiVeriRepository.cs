using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;

namespace SuperFilm.Enerji.Repository
{
    public interface IEnerjiVeriRepository<TDbContext> where TDbContext : DbContext
    {
        Task<List<SayacVeri>> GetDailyAsync(DateTime gun, int sayacId);
        Task<List<SayacVeri>> GetMonthlyAsync (DateTime ay, int sayacId);
    }

}
