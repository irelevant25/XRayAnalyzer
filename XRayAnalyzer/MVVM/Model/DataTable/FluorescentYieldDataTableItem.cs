using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.DataTable
{
    public class FluorescentYieldDataTableItem
    {
        public int Number { get; set; }

        public string? Name { get; set; }

        public string? Symbol { get; set; }

        public string? Line { get; set; }

        public double Probability { get; set; }
    }
}
