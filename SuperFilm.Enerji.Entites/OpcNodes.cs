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
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string Code { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string Description { get; set; }
        [Column(TypeName = "varchar(255)")]
        [StringLength(255)]
        public string NodeId { get; set; }
        public int NodeNameSpace { get; set; } = 3;
        public int AttributeId { get; set; } = 13;

    }
}
