using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Calibration
{
    public class CalibrationResponse
    {
        [JsonPropertyName("slope")]
        public double Slope { get; set; }

        [JsonPropertyName("intercept")]
        public double Intercept { get; set; }

        [JsonPropertyName("rvalue")]
        public double Rvalue { get; set; }

        [JsonPropertyName("pvalue")]
        public double Pvalue { get; set; }

        [JsonPropertyName("stderr")]
        public double Stderr { get; set; }

        [JsonPropertyName("intercept_stderr")]
        public double InterceptStderr { get; set; }
    }
}
