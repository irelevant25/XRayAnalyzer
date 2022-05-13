using System.Text.Json.Serialization;
using XRayAnalyzer.Objects;

namespace XRayAnalyzer.MVVM.Model
{
    public class SignalPoint : PropertyChangedBaseModel
    {
        public int Channel { get; set; }

        public double Value { get; set; }

        [JsonIgnore]
        public double Energy { get { return Helper.ChannelToEnergy(Channel); } }

        [JsonIgnore]
        public double ValueLog { get { return Helper.ValueToValueLog(Value); } }
    }
}
