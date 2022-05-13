using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Calibration
{
    public class CalibrationProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "calibration";

        [JsonPropertyName("data_for_predict")]
        public List<int>? DataToPredict { get; set; }

        [JsonPropertyName("data_x")]
        public List<int>? Channels { get; set; }

        [JsonPropertyName("data_y")]
        public List<double>? Energies { get; set; }
    }
}
