using System.Collections.Generic;

namespace XRayAnalyzer.MVVM.Model
{
    public class Element : PropertyChangedBaseModel
    {
        public ElementInfo? Info { get; set; }
        public List<ElementLine>? Lines { get; set; }
    }
}
