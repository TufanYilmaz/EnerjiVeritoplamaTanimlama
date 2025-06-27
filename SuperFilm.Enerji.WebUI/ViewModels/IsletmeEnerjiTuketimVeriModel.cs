using System.Text.Json.Serialization;

namespace SuperFilm.Enerji.WebUI.ViewModels
{
    public class IsletmeEnerjiTuketimVeriModel
    {
        /*
          <th>Kod</th>
                <th>Aciklama</th>
                <th>Başlangıç Tarihi</th>
                <th>Bitiş Tarihi</th>
                <th>Değer</th>
         */
        public int Id { get; set; }
        [JsonPropertyName("Aciklama")]
        public string Aciklama { get; set; }
        [JsonPropertyName("StartDate")]
        public string StartDate { get; set; }
        [JsonPropertyName("StartValue")]
        public decimal StartValue { get; set; }

        [JsonPropertyName("EndDate")]
        public string EndDate { get; set; }
        [JsonPropertyName("EndValue")]
        public decimal EndValue { get; set; }

        [JsonPropertyName("Value")]
        public decimal Value => EndValue - StartValue;

        [JsonPropertyName("TotalValue")]
        public decimal TotalValue { get; set; }
    }
}
