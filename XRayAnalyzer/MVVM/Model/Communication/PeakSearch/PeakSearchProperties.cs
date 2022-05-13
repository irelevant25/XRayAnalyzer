using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.PeakSearch
{
    internal class PeakSearchProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "get_peaks";

        [JsonPropertyName("data")]
        public List<double>? Data { get; set; }

        [JsonPropertyName("height")]
        public double? Height { get; set; } = null;

        [JsonPropertyName("threshold")]
        public double? Threshold { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("prominence")]
        public double Prominence { get; set; }

        [JsonPropertyName("width")]
        public List<double>? Width { get; set; }

        [JsonPropertyName("wlen")]
        public double Wlen { get; set; }
    }
}
