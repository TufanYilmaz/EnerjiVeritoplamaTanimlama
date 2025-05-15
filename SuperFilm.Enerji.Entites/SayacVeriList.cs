using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public class SayacVeriList
    {
        public string? Kod { get; set; }
        public string? SayacTanimi { get; set; }
        public string? Description { get; set; }
       
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deger { get; set; }
        public DateTime NormalizeDate { get; set; }


    }
}
