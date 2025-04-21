using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuperFilm.Enerji.Entites
{
    public class SayacDbContext : DbContext
    {
        public DbSet<SayacVeri> SayacVerileri { get; set; }
        public DbSet<SayacTanimlari> sayaclar { get; set; }

        public SayacDbContext(DbContextOptions<SayacDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
