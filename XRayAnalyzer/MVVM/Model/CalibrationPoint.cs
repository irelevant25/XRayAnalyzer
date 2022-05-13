using System.Text.Json.Serialization;

namespace XRayAnalyzer.MVVM.Model
{
    public class CalibrationPoint : PropertyChangedBaseModel
    {
        public int Channel { get; set; }

        [JsonIgnore]
        public ElementInfo? ElementInfo { get; set; }

        [JsonIgnore]
        public ElementLine? ElementLine { get; set; }

        public double? Energy
        {
            get
            {
                return ElementLine?.Energy;
            }
            set
            {
                if (value is not null)
                {
                    ElementLine = new ElementLine()
                    {
                        Energy = (double)value
                    };
                }
            }
        }
    }
}
