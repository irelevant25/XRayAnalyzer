using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class DetectorEfficiency : PropertyChangedBaseModel
    {
        [JsonPropertyName("detector")]
        public string? Detector { get; set; }

        [JsonPropertyName("window")]
        public string? Window { get; set; }

        [JsonPropertyName("energy")]
        public List<double>? Energies { get; set; }

        [JsonPropertyName("total_attenuation")]
        public List<double>? TotalAttenuations { get; set; }

        [JsonPropertyName("total_photofraction")]
        public List<double>? TotalPhotofractions { get; set; }

        [JsonIgnore]
        public string DetectorIdentifier
        {
            get
            {
                return Detector + " - " + Window;
            }
        }
    }
}
