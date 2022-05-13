using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class FluorescentYield : PropertyChangedBaseModel
    {
        [JsonPropertyName("element")]
        public int Element { get; set; }

        [JsonPropertyName("probabilities")]
        public List<LineProbability>? Probabilities { get; set; }
    }
}
