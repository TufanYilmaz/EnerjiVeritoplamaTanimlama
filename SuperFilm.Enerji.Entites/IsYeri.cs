using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    [Table("IS_YERI")]
    public class IsYeri
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        [StringLength(20)]
        public string Kod { get; set; }
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string Ad { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string Aciklama { get; set; }

        public IEnumerable<SayacTanimlari> SayacTanimlari { get; set; }

    }
}
