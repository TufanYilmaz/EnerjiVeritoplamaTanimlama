using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public class EnerjiDbContext : DbContext
    {
        //Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
        //Server=BSPSQLLST.sankoholding.local;Database=Super_Enerji;User Id=Super_Enerji;Password=pscV0Fj1F5lf;
        DbSet<IsletmeTanimlari> IsletmeTanimlari { get; set; }
        DbSet<SayacTanimlari> SayacTanimlari { get; set; }
        public EnerjiDbContext(DbContextOptions<EnerjiDbContext> options):base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
