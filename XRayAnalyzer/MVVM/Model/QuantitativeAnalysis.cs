using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.Objects;

namespace XRayAnalyzer.MVVM.Model
{
    public class QuantitativeAnalysis : PropertyChangedBaseModel
    {
        public ElementInfo? PrimaryElement { get; set; } = Dataset.ElementsInfo?.ToList().Find(elementInfo => elementInfo.Number == 47);

        public DetectorEfficiency? DetectorEfficiency { get; set; } = Dataset.DetectorEfficiencies?.ToList().First();

        public double XRayTubeSampleAngle { get; set; } = 22.5;

        public double DetectorSampleAngle { get; set; } = 22.5;
    }
}
