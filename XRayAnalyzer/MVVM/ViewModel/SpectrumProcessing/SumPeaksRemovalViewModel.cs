using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing
{
    public class SumPeaksRemovalViewModel : PropertyChangedBaseModel
    {
        private ObservableCollection<Peak>? peaks { get; set; } = new();
        public ObservableCollection<Peak>? Peaks
        {
            get
            {
                return peaks;
            }
            set
            {
                peaks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Peak>? sumPeaks { get; set; } = new ();
        public ObservableCollection<Peak>? SumPeaks
        {
            get
            {
                return sumPeaks;
            }
            set
            {
                sumPeaks = value;
                OnPropertyChanged();
            }
        }
    }
}
