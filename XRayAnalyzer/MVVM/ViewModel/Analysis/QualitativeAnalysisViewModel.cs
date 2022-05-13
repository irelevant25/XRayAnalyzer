using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.Analysis
{
    public class QualitativeAnalysisViewModel : PropertyChangedBaseModel
    {
        private QualitativeAnalysisItem? selectedQualitativeAnalysisItem;
        public QualitativeAnalysisItem? SelectedQualitativeAnalysisItem
        {
            get
            {
                return selectedQualitativeAnalysisItem;
            }
            set
            {
                selectedQualitativeAnalysisItem = value;
                OnPropertyChanged();
            }
        }

        private QualitativeAnalysis qualitativeAnalysisProperties = new();
        public QualitativeAnalysis QualitativeAnalysisProperties
        {
            get
            {
                return qualitativeAnalysisProperties;
            }
            set
            {
                qualitativeAnalysisProperties = value;
                OnPropertyChanged();
            }
        }

        private bool analysisRunning = false;
        public bool AnalysisRunning
        {
            get
            {
                return analysisRunning;
            }
            set
            {
                analysisRunning = value;
                OnPropertyChanged();
            }
        }
    }
}
