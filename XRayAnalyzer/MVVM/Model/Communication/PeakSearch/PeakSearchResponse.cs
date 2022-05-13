using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.PeakSearch
{
    internal class PeakSearchResponse
    {
        [JsonPropertyName("peaks_indexes")]
        public List<int>? PeaksIndexes { get; set; }

        [JsonPropertyName("left_bases_indexes")]
        public List<int>? LeftBasesIndexes { get; set; }

        [JsonPropertyName("right_bases_indexes")]
        public List<int>? RightBasesIndexes { get; set; }
    }
}
