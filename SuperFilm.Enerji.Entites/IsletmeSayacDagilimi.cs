using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperFilm.Enerji.Entites
{
    [Table("ISLETME_SAYAC_DAGILIMI")]
    public class IsletmeSayacDagilimi
    {
        [Key]
        public int Id { get; set; }
    }
}
