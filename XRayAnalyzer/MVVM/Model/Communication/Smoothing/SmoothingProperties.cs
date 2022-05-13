using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Smoothing
{
    public class SmoothingProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "gaussian_filter";

        [JsonPropertyName("data")]
        public List<double>? Data { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("truncate")]
        public double? Truncate { get; set; }
    }
}
