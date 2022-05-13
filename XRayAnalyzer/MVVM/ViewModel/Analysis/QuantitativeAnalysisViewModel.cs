using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.Analysis
{
    public class QuantitativeAnalysisViewModel : PropertyChangedBaseModel
    {
        public ObservableCollection<ElementInfo> ElementsInfo { get; set; } = new ObservableCollection<ElementInfo>();

        public ObservableCollection<DetectorEfficiency> DetectorEfficiencies { get; set; } = new ObservableCollection<DetectorEfficiency>();

        private ObservableCollection<QuantitativeAnalysisItem>? quantitativeAnalysisItems;
        public ObservableCollection<QuantitativeAnalysisItem>? QuantitativeAnalysisItems
        {
            get
            {
                return quantitativeAnalysisItems;
            }
            set
            {
                quantitativeAnalysisItems = value;
                OnPropertyChanged();
            }
        }

        private QuantitativeAnalysis quantitativeAnalysisProperties = new();
        public QuantitativeAnalysis QuantitativeAnalysisProperties
        {
            get
            {
                return quantitativeAnalysisProperties;
            }
            set
            {
                quantitativeAnalysisProperties = value;
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
