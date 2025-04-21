using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    [Table("SAYAC_VERI")]
    public class SayacVeri
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string? Kod { get; set; }

        [Column(TypeName = "varchar(4)")]
        [StringLength(4)]
        [Required]
        public string Yil { get; set; }
        [Column(TypeName = "varchar(2)")]
        [StringLength(2)]
        [Required]
        public string Ay { get; set; }
        [Column(TypeName = "varchar(2)")]
        [StringLength(2)]
        [Required]
        public string Gun { get; set; }
        [Column(TypeName = "varchar(6)")]
        [StringLength(6)]
        [Required]
        public string Zaman { get; set; } //hhmmss //010925
        [Required]
        public DateTime NormalizeDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deger { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public int? SayacId { get; set; }
        public int? OpcNodesId { get; set; }
        
    }
}
