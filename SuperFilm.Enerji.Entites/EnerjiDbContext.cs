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
        public DbSet<IsYeri> IsYeri { get; set; }
        public DbSet<SayacTanimlari> SayacTanimlari { get; set; }
        public DbSet<IsletmeSayacDagilimi> IsletmeSayacDagilimi { get; set; }
        public DbSet<Isletme> Isletme { get; set; }
        public DbSet<SayacVeri> SayacVeri { get; set; }
        public DbSet<OpcNodes> OpcNode { get; set; }
        public DbSet<OpcNodesIsletmeDagilimi> OpcNodesIsletmeDagilimi { get; set; }
        public DbSet<EnerjiRequestAdvance> EnerjiRequestAdvance { get; set; }
        public DbSet<EnerjiRequest> EnerjiRequest { get; set; }
        public EnerjiDbContext(DbContextOptions<EnerjiDbContext> options) : base(options) { }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
