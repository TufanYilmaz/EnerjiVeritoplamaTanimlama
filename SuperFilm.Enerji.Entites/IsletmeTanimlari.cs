using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    [Table("SAYAC_TANIMLARI")]
    public class IsletmeTanimlari
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        [StringLength(20)]
        [Required]
        public string? IsletmeKodu { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string? IsletmeAdi { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string? Aciklama { get; set; }
    }
}
