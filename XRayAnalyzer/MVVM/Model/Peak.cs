using ScottPlot.Plottable;
using System.Text.Json.Serialization;

namespace XRayAnalyzer.MVVM.Model
{
    public class Peak : PropertyChangedBaseModel
    {
        [JsonIgnore]
        public long Id { get; set; }

        public SignalPoint HighestSignalPoint { get; set; } = new();

        public SignalPoint LeftBaseSignalPoint { get; set; } = new();

        public SignalPoint RightBaseSignalPoint { get; set; } = new();

        public int? GrossArea { get; set; }

        public int? NetArea { get; set; }

        public ElementLine? ElementLine { get; set; }

        public double Quantity { get; set; }

        [JsonIgnore]
        public SignalPlotXY? SignalPlot { get; set; }

        [JsonIgnore]
        public ElementMarker? ElementMarker { get; set; }
    }
}
