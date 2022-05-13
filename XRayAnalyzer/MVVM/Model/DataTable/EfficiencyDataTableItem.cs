using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.DataTable
{
    public class EfficiencyDataTableItem
    {
        public string? Detector { get; set; }

        public string? Window { get; set; }

        public double Energy { get; set; }

        public double TotalAttenuation { get; set; }

        public double TotalPhotofraction { get; set; }
    }
}
