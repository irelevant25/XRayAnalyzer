using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Data
{
    public class XrayMassCoefficientResponse
    {
        [JsonPropertyName("data")]
        public List<XrayMassCoefficient>? XrayMassCoefficients { get; set; }
    }
}
