using System.Collections.Generic;
using System.Collections.ObjectModel;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing
{
    public class SpectrumProcessingViewModel : PropertyChangedBaseModel
    {
        private bool isLoadFileOperation;
        public bool IsLoadFileOperation
        {
            get
            {
                return isLoadFileOperation;
            }
            set
            {
                isLoadFileOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isSumPeaksRemovalOperation;
        public bool IsSumPeaksRemovalOperation
        {
            get
            {
                return isSumPeaksRemovalOperation;
            }
            set
            {
                isSumPeaksRemovalOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isSmoothingOperation;
        public bool IsSmoothingOperation
        {
            get
            {
                return isSmoothingOperation;
            }
            set
            {
                isSmoothingOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isNetExtractionOperation;
        public bool IsNetExtractionOperation
        {
            get
            {
                return isNetExtractionOperation;
            }
            set
            {
                isNetExtractionOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isBackgroundRemovalOperation;
        public bool IsBackgroundRemovalOperation
        {
            get
            {
                return isBackgroundRemovalOperation;
            }
            set
            {
                isBackgroundRemovalOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isExploreSignalOperation;
        public bool IsExploreSignalOperation
        {
            get
            {
                return isExploreSignalOperation;
            }
            set
            {
                isExploreSignalOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isEditPeaksOperation;
        public bool IsEditPeaksOperation
        {
            get
            {
                return isEditPeaksOperation;
            }
            set
            {
                isEditPeaksOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isSearchPeaksOperation;
        public bool IsSearchPeaksOperation
        {
            get
            {
                return isSearchPeaksOperation;
            }
            set
            {
                isSearchPeaksOperation = value;
                OnPropertyChanged();
            }
        }

        private bool isCalibrateDataOperation;
        public bool IsCalibrateDataOperation
        {
            get
            {
                return isCalibrateDataOperation;
            }
            set
            {
                isCalibrateDataOperation = value;
                OnPropertyChanged();
            }
        }
    }
}
