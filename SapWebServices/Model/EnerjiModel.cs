using System.Text.Json.Serialization;

namespace SapWebServices.Model
{
    public class EnerjiModel
    {
        [JsonPropertyName("UretimYeri")]
        public string? UretimYeri { get; set; }
        [JsonPropertyName("DirektDeger")]
        public double DDeger { get; set; } = 0;
        [JsonPropertyName("EndirektDeger")]
        public double EDeger { get; set; } = 0;

    }
    public class EnerjiModelDetail: EnerjiModel
    {
        public EnerjiModelDetail()
        {

        }
        public EnerjiModelDetail(EnerjiModel enerjiModel)
        {
            this.UretimYeri = enerjiModel.UretimYeri;
            this.DDeger = enerjiModel.DDeger;
            this.EDeger = enerjiModel.EDeger;
        }
        [JsonPropertyName("BaslamaZaman")]
        public string? StartDateTime { get; set; }
        [JsonPropertyName("BitisZaman")]
        public string? EndDateTime { get; set; }

    }
    public class EnerjiModelAdvanceDetail : EnerjiModel
    {
        public EnerjiModelAdvanceDetail()
        {

        }
        public EnerjiModelAdvanceDetail(EnerjiModel enerjiModel)
        {
            this.UretimYeri = enerjiModel.UretimYeri;
            this.DDeger = enerjiModel.DDeger;
            this.EDeger = enerjiModel.EDeger;
        }
		public EnerjiModelAdvanceDetail(EnerjiRequestAdvanceModelDb db)
		{
			this.UretimYeri = db.ProductionLine;
			this.DDeger = db.DDeger;
			this.EDeger = db.EDeger;
			this.StartDate = db.StartDate;
			this.StartTime = db.StartTime;
			this.EndDate = db.EndDate;
			this.EndTime = db.EndTime;
		}
		[JsonPropertyName("BaslamaTarih")]
        public string? StartDate { get; set; }
        [JsonPropertyName("BaslamaZaman")]
        public string? StartTime { get; set; }
        [JsonPropertyName("BitisTarih")]
        public string? EndDate { get; set; }
        [JsonPropertyName("BitisZaman")]
        public string? EndTime { get; set; }

    }
}
