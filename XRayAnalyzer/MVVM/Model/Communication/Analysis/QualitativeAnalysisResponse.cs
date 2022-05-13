using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Analysis
{
    public class QualitativeAnalysisResponse
    {
        [JsonPropertyName("data")]
        public List<QualitativeAnalysisItem>? Data { get; set; }
    }
}
