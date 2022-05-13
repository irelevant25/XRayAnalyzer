using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XRayAnalyzer.MVVM.Model
{
    class MCA : PropertyChangedBaseModel
    {
        [JsonPropertyName("spectrum")]
        public Dictionary<string, string>? Spectrum { get; set; }

        [JsonPropertyName("calibration_label")]
        public List<int>? CalibrationChannels { get; set; }

        [JsonPropertyName("calibration_channel")]
        public List<double>? CalibrationEnergies { get; set; }

        [JsonPropertyName("roi_start")]
        public List<int>? PeaksStart { get; set; }

        [JsonPropertyName("roi_end")]
        public List<int>? PeaksEnd { get; set; }

        [JsonPropertyName("data")]
        public List<int>? Data { get; set; }

        [JsonPropertyName("configuration")]
        public Dictionary<string, string>? Configuration { get; set; }

        [JsonPropertyName("status")]
        public Dictionary<string, string>? Status { get; set; }
    }
}
