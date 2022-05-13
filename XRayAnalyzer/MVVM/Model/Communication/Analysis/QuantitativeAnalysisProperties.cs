using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.Communication.Analysis
{
    public class QuantitativeAnalysisProperties : IPropertiesBaseInterface
    {
        [JsonIgnore]
        public string FunctionName { get; } = "quantitative_analysis";

        [JsonPropertyName("xray_mass_coefficients")]
        public List<XrayMassCoefficient>? XrayMassCoefficients { get; set; }

        [JsonPropertyName("detector_efficiencies")]
        public DetectorEfficiency? DetectorEfficiencies { get; set; }

        [JsonPropertyName("fluorescent_yields")]
        public List<FluorescentYield>? FluorescentYields { get; set; }

        [JsonPropertyName("jump_ratios")]
        public List<JumpRatio>? JumpRatios { get; set; }

        [JsonPropertyName("primary_element")]
        public int? PrimaryElement { get; set; }

        [JsonPropertyName("elements")]
        public List<int>? Elements { get; set; }

        [JsonPropertyName("elements_energies")]
        public List<double>? ElementsEnergies { get; set; }

        [JsonPropertyName("elements_areas")]
        public List<int>? ElementsAreas { get; set; }

        [JsonPropertyName("elements_radrates")]
        public List<double>? ElementsRadrates { get; set; }

        [JsonPropertyName("elements_lines")]
        public List<string>? ElementsLines { get; set; }

        [JsonPropertyName("x_ray_tube_angel")]
        public double XrayTubeAngel { get; set; }

        [JsonPropertyName("detector_angel")]
        public double DetectorAngel { get; set; }
    }
}
