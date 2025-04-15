using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    [Table("ISYERI")]
    public class IsYeri
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        [StringLength(20)]
        [Required]
        public string? Kodu { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string? Adi { get; set; }
        [Column(TypeName = "varchar(255)")] 
        [StringLength(255)]
        public string? Aciklama { get; set; }
        public IEnumerable<Isletme>? Isletmeler { get; set; }
       
    }
}
