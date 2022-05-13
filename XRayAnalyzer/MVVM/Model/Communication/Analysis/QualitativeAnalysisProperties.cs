using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Analysis
{
    public class QualitativeAnalysisProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "qualitative_analysis";

        [JsonPropertyName("elements_lines")]
        public List<ElementLine>? ElementsLines { get; set; }

        [JsonPropertyName("energies")]
        public List<double>? Energies { get; set; }

        [JsonPropertyName("energy_abs_treshold")]
        public double EnergyAbsTreshold { get; set; }

        [JsonPropertyName("rate_treshold")]
        public double RateTreshold { get; set; }
    }
}
