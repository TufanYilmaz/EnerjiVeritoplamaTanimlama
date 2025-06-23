namespace SapWebServices.Model
{
    public class EnerjiRequestModel
    {
        public string? ProductionLine { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public HelperPlants? HelperPlants { get; set; }
    }
    public class EnerjiRequestModelList
    {
        public List<EnerjiRequestSimpleModel>? EnerjiModel { get; set; }
        public HelperPlants? HelperPlants { get; set; }
    }
    public class EnerjiRequestSimpleModel
    {
        public string? ProductionLine { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal StartValue { get; set; }
        public decimal EndValue { get; set; }
        public decimal Value { get; set; }
    }
    public class EnerjiRequestAdvaceModelList
    {
        public List<EnerjiRequestAdvanceModel>? EnerjiModel { get; set; }
        public HelperPlants? HelperPlants { get; set; }
    }
    public class EnerjiRequestAdvanceModel
    {
        public string? ProductionLine { get; set; }
        public string? StartDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndDate { get; set; }
        public string? EndTime { get; set; }
    }
    public class EnerjiRequestAdvanceModelDb: EnerjiRequestAdvanceModel
    {
        public int Id { get; set; }
        public double DDeger { get; set; }
        public double EDeger { get; set; }

        public EnerjiRequestAdvanceModelDb()
        {
                
        }
        public EnerjiRequestAdvanceModelDb(EnerjiModelAdvanceDetail enerji)
        {
            this.DDeger = enerji.DDeger;
            this.EDeger = enerji.EDeger;
            this.StartDate = enerji.StartDate;
            this.StartTime = enerji.StartTime;
            this.EndDate = enerji.EndDate;
            this.EndTime = enerji.EndTime;
            this.ProductionLine = enerji.UretimYeri;


		}
    }
}
