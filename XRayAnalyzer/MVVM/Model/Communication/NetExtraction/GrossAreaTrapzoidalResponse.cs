using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.NetExtraction
{
    public class GrossAreaTrapezoidalResponse
    {
        [JsonPropertyName("data")]
        public List<double>? Data { get; set; }
    }
}
