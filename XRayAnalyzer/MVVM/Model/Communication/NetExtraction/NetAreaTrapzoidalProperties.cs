using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.NetExtraction
{
    public class NetAreaTrapezoidalProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "net_area_Trapezoidal";

        [JsonPropertyName("data_x")]
        public List<double>? DataX { get; set; }

        [JsonPropertyName("data_y")]
        public List<double>? DataY { get; set; }
    }
}
