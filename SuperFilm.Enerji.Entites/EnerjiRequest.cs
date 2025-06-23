using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public class EnerjiRequest
    {
        public int Id { get; set; }
        public List<EnerjiRequestAdvance>? EnerjiRequestAdvanceBody { get; set; }
        [Column(TypeName = "smalldatetime")]

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
