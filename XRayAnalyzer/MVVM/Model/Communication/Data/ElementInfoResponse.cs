using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.Model.Communication.Data
{
    internal class ElementInfoResponse
    {
        [JsonPropertyName("data")]
        public List<ElementInfo>? ElementsInfo { get; set; }
    }
}
