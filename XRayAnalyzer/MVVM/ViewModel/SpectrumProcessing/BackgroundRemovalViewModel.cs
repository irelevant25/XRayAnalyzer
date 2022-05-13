using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing
{
    public class BackgroundRemovalViewModel : PropertyChangedBaseModel
    {
        private ObservableCollection<SignalPoint>? backgroundPoints;
        public ObservableCollection<SignalPoint>? BackgroundPoints
        {
            get
            {
                return backgroundPoints;
            }
            set
            {
                backgroundPoints = value;
                OnPropertyChanged();
            }
        }

        private IPlottable? backgroundPlot;
        public IPlottable? BackgroundPlot
        {
            get
            {
                return backgroundPlot;
            }
            set
            {
                backgroundPlot = value;
                OnPropertyChanged();
            }
        }

        private bool backgroundRemovalLivePreview = false;
        public bool BackgroundRemovalLivePreview
        {
            get
            {
                return backgroundRemovalLivePreview;
            }
            set
            {
                backgroundRemovalLivePreview = value;
                OnPropertyChanged();
            }
        }

        private BackgroundRemoval backgroundRemovalProperties = new();
        public BackgroundRemoval BackgroundRemovalProperties
        {
            get
            {
                return backgroundRemovalProperties;
            }
            set
            {
                backgroundRemovalProperties = value;
                OnPropertyChanged();
            }
        }
    }
}
