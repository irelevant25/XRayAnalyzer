using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class XrayMassCoefficient : PropertyChangedBaseModel
    {
        [JsonPropertyName("element")]
        public int Element { get; set; }

        [JsonPropertyName("energy_unit")]
        public string? EnergyUnit { get; set; }

        [JsonPropertyName("energy")]
        public List<double>? Energies { get; set; }

        [JsonPropertyName("mass_attenuation_coefficient_unit")]
        public string? MassAttenuationCoefficientUnit { get; set; }

        [JsonPropertyName("mass_attenuation_coefficient")]
        public List<double>? MassAttenuationCoefficients { get; set; }

        [JsonPropertyName("mass_absorption_coefficient_unit")]
        public string? MassAbsorptionCoefficientUnit { get; set; }

        [JsonPropertyName("mass_absorption_coefficient")]
        public List<double>? MassAbsorptionCoefficients { get; set; }
    }
}
