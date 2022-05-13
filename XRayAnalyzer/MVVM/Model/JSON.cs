using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model.Communication.Analysis;
using XRayAnalyzer.MVVM.Model.Communication.BackgroundRemoval;
using XRayAnalyzer.MVVM.Model.Communication.Calibration;
using XRayAnalyzer.MVVM.Model.Communication.MCA;

namespace XRayAnalyzer.MVVM.Model
{
    public class JSON
    {
        public List<SignalPoint>? ReferencePoints { get; set; }

        public List<Peak>? ReferencePeaks { get; set; }

        public List<Peak>? SumPeaks { get; set; }

        public MCAResponse? MCAFile { get; set; }

        public BackgroundRemovalZhangfitProperties? BackgroundProperties { get; set; }

        public List<SignalPoint>? Background { get; set; }

        public CalibrationProperties? CalibrationProperties { get; set; }

        public Calibration? Calibration { get; set; }

        public QualitativeAnalysisProperties? QualitativeAnalysisProperties { get; set; }

        public List<QualitativeAnalysisItem>? QualitativeAnalysis { get; set; }

        public QuantitativeAnalysisProperties? QuantitativeAnalysisProperties { get; set; }

        public List<QuantitativeAnalysisItem>? QuantitativeAnalysis { get; set; }

        // Quantitaty analysis is representet byt attribute Quantity in Peak model

        [JsonIgnore]
        public double? LogBase { get; set; }

        [JsonIgnore]
        public List<SignalPoint>? ProcessedPoints { get; set; }

        [JsonIgnore]
        public List<Peak>? ProcessedPeaks { get; set; }
    }
}
