using System.Text.Json.Serialization;

namespace XRayAnalyzer.MVVM.Model
{
    public class ElementLine : PropertyChangedBaseModel
    {
        [JsonPropertyName("element")]
        public int Element { get; set; }

        [JsonPropertyName("line")]
        public string? Line { get; set; }

        [JsonPropertyName("energy")]
        public double Energy { get; set; }

        [JsonPropertyName("rate")]
        public double Rate { get; set; }

        [JsonIgnore]
        public ElementInfo? Info { get; set; }
    }
}
