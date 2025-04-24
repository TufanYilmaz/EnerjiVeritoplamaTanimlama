using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public enum OrderBy
    {
        Ascending = 1,
        Descending = 2
    }
    public class SayacVeriRequest
    {
        [Key]
        public int Id { get; set; }
        public int DataTypeId { get; set; }
        public int? SayacOpcNodesID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int NumData { get; set; }
        public OrderBy OrderBy { get; set; } = OrderBy.Descending;
    }
}
