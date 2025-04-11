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
    public class SayacTanimlari
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        [StringLength(20)]
        [Required]
        public string SayacKodu { get; set; }

        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required]
        public string SayacTanimi { get; set; }

        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required]
        public string SayacAciklama { get; set; }

        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        [Required]
        public string SayacYeri { get; set; }
        [Required]
        public int IsYeriId { get; set; } // Foreign Key
    }
}
