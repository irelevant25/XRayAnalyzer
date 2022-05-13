using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing
{
    public class EditPeaksViewModel : PropertyChangedBaseModel
    {
        private bool isSearchingPeak;
        public bool IsSearchingPeak
        {
            get
            {
                return isSearchingPeak;
            }
            set
            {
                isSearchingPeak = value;
                OnPropertyChanged();
            }
        }

        private bool isSelectOnlyPeak = true;
        public bool IsSelectOnlyPeak
        {
            get
            {
                return isSelectOnlyPeak;
            }
            set
            {
                isSelectOnlyPeak = value;
                OnPropertyChanged();
            }
        }

        private bool isAddingPeak;
        public bool IsAddingPeak
        {
            get
            {
                return isAddingPeak;
            }
            set
            {
                isAddingPeak = value;
                OnPropertyChanged();
            }
        }

        private bool isEditingPeak;
        public bool IsEditingPeak
        {
            get
            {
                return isEditingPeak;
            }
            set
            {
                isEditingPeak = value;
                OnPropertyChanged();
            }
        }

        private bool isRemovingPeak;
        public bool IsRemovingPeak
        {
            get
            {
                return isRemovingPeak;
            }
            set
            {
                isRemovingPeak = value;
                OnPropertyChanged();
            }
        }

        private bool peakSearchLivePreview = false;
        public bool PeakSearchLivePreview
        {
            get
            {
                return peakSearchLivePreview;
            }
            set
            {
                peakSearchLivePreview = value;
                OnPropertyChanged();
            }
        }

        private PeakSearch peakSearch = new();
        public PeakSearch PeakSearch
        {
            get
            {
                return peakSearch;
            }
            set
            {
                peakSearch = value;
                OnPropertyChanged();
            }
        }
    }
}
