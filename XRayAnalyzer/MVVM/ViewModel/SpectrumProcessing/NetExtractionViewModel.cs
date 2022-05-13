using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing
{
    public class NetExtractionViewModel : PropertyChangedBaseModel
    {
        private ObservableCollection<Peak>? peaks;
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
    }
}
