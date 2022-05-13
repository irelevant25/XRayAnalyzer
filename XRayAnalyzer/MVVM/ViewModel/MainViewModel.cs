using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.View;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel
{
    class MainViewModel : PropertyChangedBaseModel
    {
        private object? selectedView;
        public object? SelectedView
        {
            get { return selectedView; }
            set { selectedView = value; OnPropertyChanged(); }
        }
    }
}
