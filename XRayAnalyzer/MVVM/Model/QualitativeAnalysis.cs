using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class QualitativeAnalysis : PropertyChangedBaseModel
    {
        public double EnergyAbsTreshold { get; set; } = 0.04;

        public double RateTreshold { get; set; } = 0.02;
    }
}
