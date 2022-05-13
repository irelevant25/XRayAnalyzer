using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace XRayAnalyzer.MVVM.Model
{
    public class Calibration : PropertyChangedBaseModel
    {
        public ObservableCollection<CalibrationPoint> CalibrationPoints { get; set; } = new();

        public double Slope { get; set; }

        public double Intercept { get; set; }

        public double Rvalue { get; set; }

        public double Pvalue { get; set; }

        public double Stderr { get; set; }

        public double InterceptStderr { get; set; }
    }
}
