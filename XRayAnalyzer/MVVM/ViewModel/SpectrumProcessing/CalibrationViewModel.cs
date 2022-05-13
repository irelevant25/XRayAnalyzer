using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing
{
    public class CalibrationViewModel : PropertyChangedBaseModel
    {
        public ObservableCollection<Element> Elements { get; set; } = new ObservableCollection<Element>();

        private Calibration calibration = new();
        public Calibration Calibration
        {
            get
            {
                return calibration;
            }
            set
            {
                calibration = value;
                OnPropertyChanged();
            }
        }

        private Element? calibrationElementSelected;
        public Element? CalibrationElementSelected
        {
            get
            {
                return calibrationElementSelected;
            }
            set
            {
                calibrationElementSelected = value;
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

        private ElementLine? calibrationLineSelected;
        public ElementLine? CalibrationLineSelected
        {
            get
            {
                return calibrationLineSelected;
            }
            set
            {
                calibrationLineSelected = value;
                OnPropertyChanged();
            }
        }

        private CalibrationPoint? calibrationPointSelected;
        public CalibrationPoint? CalibrationPointSelected
        {
            get
            {
                return calibrationPointSelected;
            }
            set
            {
                calibrationPointSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
