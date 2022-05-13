using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model.DataTable
{
    public class ElementLineDataTableItem
    {
        public int Number { get; set; }

        public string? Name { get; set; }

        public string? Symbol { get; set; }

        public string? Line { get; set; }

        public double Energy { get; set; }

        public double Rate { get; set; }
    }
}
