using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    [Table("ISLETME")]
    public class Isletme
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        [StringLength(20)]
        [Required]
        public string Kod { get; set; }
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        [Required]
        public string Ad { get; set; }
        [StringLength(50)]
        public string? Tip { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string? Aciklama { get; set; }
        [Required]
        public int IsyeriId { get; set; }
        public IsYeri? Isyeri { get; set; }
        public IEnumerable<SayacTanimlari>? SayacTanimlari { get; set; }

    }
}
