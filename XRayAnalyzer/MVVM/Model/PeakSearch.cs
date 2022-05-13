using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class PeakSearch : PropertyChangedBaseModel
    {
        public double Distance { get; set; } = 10;

        public double Prominence { get; set; } = 80;

        public double WidthFrom { get; set; } = 0;

        public double WidthTo { get; set; } = 15;

        public double Wlen { get; set; } = 30;

        public List<Peak>? Peaks { get; set; }
    }
}
