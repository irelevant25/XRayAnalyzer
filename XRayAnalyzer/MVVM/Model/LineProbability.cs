using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class LineProbability : PropertyChangedBaseModel
    {
        [JsonPropertyName("line")]
        public string? Line { get; set; }

        [JsonPropertyName("probability")]
        public double Probability { get; set; }
    }
}
