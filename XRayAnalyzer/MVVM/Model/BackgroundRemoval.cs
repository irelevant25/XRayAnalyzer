using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class BackgroundRemoval : PropertyChangedBaseModel
    {
        public List<double>? Points { get; set; }

        public double Lambda { get; set; } = 100;

        public int Itermax { get; set; } = 15;
    }
}
