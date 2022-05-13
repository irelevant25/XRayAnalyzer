using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.BackgroundRemoval
{
    [Obsolete]
    public class BackgroundDetectionProperties
    {
        [JsonPropertyName("points")]
        public List<double>? Points { get; set; }

        [JsonPropertyName("lam")]
        public double? Lambda { get; set; } = null;

        [JsonPropertyName("p")]
        public double? P { get; set; } = null;

        [JsonPropertyName("niter")]
        public int? Iterations { get; set; } = null;
    }
}
