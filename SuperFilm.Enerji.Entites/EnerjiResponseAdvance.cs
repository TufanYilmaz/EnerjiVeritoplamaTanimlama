using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.Entites
{
    public class EnerjiResponseAdvance
    {
        public int Id { get; set; }
        [JsonPropertyName("BaslamaTarih")]
        [Column(TypeName = "varchar(10)")]
        public string? StartDate { get; set; }

        [JsonPropertyName("BaslamaZaman")]
        [Column(TypeName = "varchar(10)")]
        public string? StartTime { get; set; }

        [JsonPropertyName("BitisTarih")]
        [Column(TypeName = "varchar(10)")]
        public string? EndDate { get; set; }

        [JsonPropertyName("BitisZaman")]
        [Column(TypeName = "varchar(10)")]
        public string? EndTime { get; set; }

        [JsonPropertyName("UretimYeri")]
        [Column(TypeName = "varchar(10)")]
        public string? UretimYeri { get; set; }

        [JsonPropertyName("DirektDeger")]
        public double DDeger { get; set; } = 0;

        [JsonPropertyName("EndirektDeger")]
        public double EDeger { get; set; } = 0;

        public int EnerjiRequestId { get; set; }
        public EnerjiRequest EnerjiRequest { get; set; }

    }
}
