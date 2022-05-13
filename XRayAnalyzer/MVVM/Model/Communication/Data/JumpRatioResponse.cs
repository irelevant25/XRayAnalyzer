using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Data
{
    public class JumpRatioResponse
    {
        [JsonPropertyName("data")]
        public List<JumpRatio>? JumpRatios { get; set; }
    }
}
