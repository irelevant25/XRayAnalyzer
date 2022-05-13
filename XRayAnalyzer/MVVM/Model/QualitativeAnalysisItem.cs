using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class QualitativeAnalysisItem : PropertyChangedBaseModel
    {
        [JsonPropertyName("energy")]
        public double Energy { get; set; }

        [JsonPropertyName("possible_matches")]
        public List<ElementLine>? PossibleMatches { get; set; }

        [JsonPropertyName("best_matches")]
        public List<ElementLine>? BestMatches { get; set; }
    }
}
