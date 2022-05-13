using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class JumpRatio : PropertyChangedBaseModel
    {
        [JsonPropertyName("element")]
        public int Element { get; set; }

        [JsonPropertyName("jump_ratios")]
        public List<LineJumpRatio>? JumpRatios { get; set; }
    }
}
