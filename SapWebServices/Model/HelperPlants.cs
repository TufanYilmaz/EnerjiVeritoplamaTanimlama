using System.Text.Json.Serialization;
namespace SapWebServices.Model
{
    public class HelperPlants
    {
        [JsonPropertyName("W01")]
        public string? MSW01 { get; set; }
        [JsonPropertyName("W02")]
        public string? MSW02 { get; set; }
        [JsonPropertyName("W03")]
        public string? MSW03 { get; set; }
        [JsonPropertyName("C01")]
        public string? MSC01 { get; set; }
        [JsonPropertyName("BW1")]
        public string? MSBW1 { get; set; }
        [JsonPropertyName("BW2")]
        public string? MSBW2 { get; set; }
        [JsonPropertyName("M01")]
        public string? MSM01 { get; set; }
        [JsonPropertyName("M02")]
        public string? MSM02 { get; set; }
        [JsonPropertyName("M03")]
        public string? MSM03 { get; set; }
        [JsonPropertyName("M04")]
        public string? MSM04 { get; set; }
        [JsonPropertyName("M05")]
        public string? MSM05 { get; set; }
        [JsonPropertyName("M06")]
        public string? MSM06 { get; set; }
        [JsonPropertyName("K01")]
        public string? MSK01 { get; set; }
        [JsonPropertyName("TE01")]
        public string? MSTE1 { get; set; }
        [JsonPropertyName("TE02")]
        public string? MSTE2 { get; set; }
        [JsonPropertyName("YIS")]
        public string? MSYIS { get; set; }
    }
}
