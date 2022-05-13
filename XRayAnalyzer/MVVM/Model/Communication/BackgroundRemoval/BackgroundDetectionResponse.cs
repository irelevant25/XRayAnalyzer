using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XRayAnalyzer.MVVM.Model.Communication.BackgroundRemoval
{
    [Obsolete]
    public class BackgroundDetectionResponse
    {
        [JsonPropertyName("data")]
        public List<double>? Data { get; set; }
    }
}
