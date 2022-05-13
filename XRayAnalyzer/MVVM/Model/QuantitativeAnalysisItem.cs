using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class QuantitativeAnalysisItem : PropertyChangedBaseModel
    {
        [JsonPropertyName("energy")]
        public double Energy { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonIgnore]
        public Peak? Peak { get; set; }
    }
}
