using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    [Table("OPC_NODES_ISLETME_DAGILIMI")]
    public class OpcNodesIsletmeDagilimi
    {
        [Key]
        public int Id { get; set; }
        public int OpcNodesId { get; set; }
        public OpcNodes OpcNodes { get; set; }
        public int IsletmeId { get; set; }
        public Isletme Isletme { get; set; }
        public char Islem { get; set; }
        [Column(TypeName = "decimal(7,6)")]
        public decimal Carpan { get; set; }

    }
}
