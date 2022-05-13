using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class LineJumpRatio : PropertyChangedBaseModel
    {
        [JsonPropertyName("line")]
        public string? Line { get; set; }

        [JsonPropertyName("jump_ratio")]
        public double Ratio { get; set; }
    }
}
