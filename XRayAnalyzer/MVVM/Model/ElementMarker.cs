using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XRayAnalyzer.MVVM.Model
{
    public class ElementMarker : PropertyChangedBaseModel
    {
        public MarkerShape MarkerShape { get; set; } = MarkerShape.eks;

        public double MarkerSize { get; set; } = 5;

        public Color MarkerColor { get; set; } = Color.Black;

        public Color TextColor { get; set; } = Color.Black;

        public float TextSize { get; set; } = 15;

        public string TextLabel { get; set; } = string.Empty;

        [JsonIgnore]
        public MarkerPlot? MarkerPlot { get; set; }

        [JsonIgnore]
        public Text? TextPlot { get; set; }
    }
}
