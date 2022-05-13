using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.Analysis
{
    public class AnalysisViewModel : PropertyChangedBaseModel
    {
        private bool isQualitativeAnalysisOperation;
        public bool IsQualitativeAnalysisOperation
        {
            get
            {
                return isQualitativeAnalysisOperation;
            }
            set
            {
                isQualitativeAnalysisOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isQuantitativeAnalysisOperation;
        public bool IsQuantitativeAnalysisOperation
        {
            get
            {
                return isQuantitativeAnalysisOperation;
            }
            set
            {
                isQuantitativeAnalysisOperation = value;
                OnPropertyChanged();
            }
        }
    }
}
