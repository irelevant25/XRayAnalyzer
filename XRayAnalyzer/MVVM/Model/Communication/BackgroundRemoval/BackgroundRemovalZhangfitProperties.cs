using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.BackgroundRemoval
{
    public class BackgroundRemovalZhangfitProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "background_removal_zhangfit";

        [JsonPropertyName("data")]
        public List<double>? Data { get; set; }

        [JsonPropertyName("lam")]
        public double? Lambda { get; set; } = null;

        [JsonPropertyName("itermax")]
        public int? Itermax { get; set; } = null;
    }
}
