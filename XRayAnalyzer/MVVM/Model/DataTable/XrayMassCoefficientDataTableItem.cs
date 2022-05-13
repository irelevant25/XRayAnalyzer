using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.DataTable
{
    public class XrayMassCoefficientDataTableItem
    {
        public int Number { get; set; }

        public string? Name { get; set; }

        public string? Symbol { get; set; }

        public string? EnergyUnit { get; set; }

        public double Energy { get; set; }

        public string? MassAttenuationCoefficientUnit { get; set; }

        public double MassAttenuationCoefficient { get; set; }

        public string? MassAbsorptionCoefficientUnit { get; set; }

        public double MassAbsorptionCoefficient { get; set; }
    }
}
