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
        [Column(TypeName = "varchar(4)")]
        [StringLength(4)]
        [Required]
        public string Yil { get; set; }
        [Column(TypeName = "varchar(2)")]
        [StringLength(2)]
        [Required]
        public string Ay { get; set; }
        [StringLength(2)]
        [Required]
        public string Gun { get; set; }
        [Column(TypeName = "varchar(6)")]
        [StringLength(6)]
        [Required]
        public string Zaman { get; set; }
        [Required]
        public DateTime NormalizeDate { get; set; }
        [Required]
        public decimal Deger { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public SayacTanimlari Sayac { get; set; }
    }
}
