using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing
{
    public class LoadFileViewModel : PropertyChangedBaseModel
    {
        private bool canChangeXAxis;
        public bool CanChangeXAxis
        {
            get
            {
                return canChangeXAxis;
            }
            set
            {
                canChangeXAxis = value;
                OnPropertyChanged();
            }
        }

        private bool canChangeYAxis;
        public bool CanChangeYAxis
        {
            get
            {
                return canChangeYAxis;
            }
            set
            {
                canChangeYAxis = value;
                OnPropertyChanged();
            }
        }

        private string? selectedFile;
        public string? SelectedFile
        {
            get
            {
                return selectedFile;
            }
            set
            {
                selectedFile = value;
                OnPropertyChanged();
            }
        }

        private bool canSelectPeak = true;
        public bool CanSelectPeak
        {
            get
            {
                return canSelectPeak;
            }
            set
            {
                canSelectPeak = value;
                OnPropertyChanged();
            }
        }

        private int? gain;
        public int? Gain
        {
            get
            {
                return gain;
            }
            set
            {
                gain = value;
                OnPropertyChanged();
            }
        }

        private int? peaksCount;
        public int? PeaksCount
        {
            get
            {
                return peaksCount;
            }
            set
            {
                peaksCount = value;
                OnPropertyChanged();
            }
        }

        private bool? calibrated;
        public bool? Calibrated
        {
            get
            {
                return calibrated;
            }
            set
            {
                calibrated = value;
                OnPropertyChanged();
            }
        }

        private bool? backgroundRemoved;
        public bool? BackgroundRemoved
        {
            get
            {
                return backgroundRemoved;
            }
            set
            {
                backgroundRemoved = value;
                OnPropertyChanged();
            }
        }

        private bool? sumPeaksRemoved;
        public bool? SumPeaksRemoved
        {
            get
            {
                return sumPeaksRemoved;
            }
            set
            {
                sumPeaksRemoved = value;
                OnPropertyChanged();
            }
        }

        private bool? smoothed;
        public bool? Smoothed
        {
            get
            {
                return smoothed;
            }
            set
            {
                smoothed = value;
                OnPropertyChanged();
            }
        }

        private bool? netExtracted;
        public bool? NetExtracted
        {
            get
            {
                return netExtracted;
            }
            set
            {
                netExtracted = value;
                OnPropertyChanged();
            }
        }

        private bool? qualitativeAnalysis;
        public bool? QualitativeAnalysis
        {
            get
            {
                return qualitativeAnalysis;
            }
            set
            {
                qualitativeAnalysis = value;
                OnPropertyChanged();
            }
        }

        private bool? quantitativeAnalysis;
        public bool? QuantitativeAnalysis
        {
            get
            {
                return quantitativeAnalysis;
            }
            set
            {
                quantitativeAnalysis = value;
                OnPropertyChanged();
            }
        }
    }
}
