using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{

    [Table("ISLETME_TANIMLARI")]
    public class SayacTanimlari
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        [StringLength(20)]
        [Required]
        public string SayacKodu { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string SayacTanimi { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string SayacYeri { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string SayacAciklama { get; set; }
    }
}
