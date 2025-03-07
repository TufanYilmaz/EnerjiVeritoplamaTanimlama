using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    [Table("OPC_NODES")]
    public class OpcNodes
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string NodeId { get; set; }
        public int NodeNameSpace { get; set; }
        public int AttributeId { get; set; } = 3;
    }
}
