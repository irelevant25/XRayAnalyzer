using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XRayAnalyzer.Controls;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.Communication.Calibration;
using XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    public partial class CalibrationView : UserControl
    {
        public CalibrationViewModel ViewModel { get; set; } = new CalibrationViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public CalibrationView()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Dataset.ElementsInfo?.ToList().ForEach((ElementInfo elementInfo) =>
            {
                Element element = new()
                {
                    Info = elementInfo,
                    Lines = Dataset.ElementsLines?.ToList().Where(line => line.Element == elementInfo.Number).ToList()
                };
                if (element.Lines != null && element.Lines.Count > 0)
                {
                    foreach (ElementLine line in element.Lines)
                    {
                        line.Energy = Math.Round(line.Energy, 3);
                    }
                    ViewModel.Elements.Add(element);
                }
            });
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (WpfPlotControl is null) return;

            if ((bool)e.NewValue)
            {
                // To visible
                if (MainModel.Data.Calibration is not null && ViewModel.Calibration.CalibrationPoints.Count == 0)
                {
                    ViewModel.Calibration = MainModel.Data.Calibration;
                }

                WpfPlotControl.SetBinding(PlotControl.ComboBox_XAxisValue_IsEnabled_Property, new Binding(nameof(ViewModel.CanChangeXAxis)) { Source = ViewModel });
                WpfPlotControl.SetBinding(PlotControl.ComboBox_YAxisValue_IsEnabled_Property, new Binding(nameof(ViewModel.CanChangeYAxis)) { Source = ViewModel });
                WpfPlotControl.SetBinding(PlotControl.CanSelectPeak_Property, new Binding(nameof(ViewModel.CanSelectPeak)) { Source = ViewModel });

                ViewModel.CanChangeXAxis = MainModel.Data.Calibration is not null;
                ViewModel.CanChangeYAxis = MainModel.Data.ReferencePoints is not null && MainModel.Data.ReferencePoints.Count > 0;
            }
            else
            {
                //To not visible
                BindingOperations.ClearAllBindings(WpfPlotControl);
                WpfPlotControl?.ResetSelectedPeak();
            }
        }

        private void Button_AddCalibrationPoint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToInt32(textBoxChannel.Text);
            }
            catch
            {
                MessageBox.Show("Zadaná hodnota kanálu nie je validná.", "Calibration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                textBoxEnergy.Text = textBoxEnergy.Text.Replace(".", ",");
                Convert.ToDouble(textBoxEnergy.Text);
            }
            catch
            {
                MessageBox.Show("Zadaná hodnota energie nie je validná.", "Calibration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ViewModel.CalibrationLineSelected?.Energy == Convert.ToDouble(textBoxEnergy.Text))
            {
                ViewModel.Calibration.CalibrationPoints.Add(new CalibrationPoint()
                {
                    ElementInfo = ViewModel.CalibrationElementSelected?.Info,
                    ElementLine = ViewModel.CalibrationLineSelected,
                    Channel = Convert.ToInt32(textBoxChannel.Text)
                });
            }
            else
            {
                ViewModel.Calibration.CalibrationPoints.Add(new CalibrationPoint()
                {
                    ElementLine = new()
                    {
                        Energy = Convert.ToDouble(textBoxEnergy.Text)
                    },
                    Channel = Convert.ToInt32(textBoxChannel.Text)
                });
            }

            textBoxChannel.Text = string.Empty;
            textBoxEnergy.Text = string.Empty;

            ViewModel.CalibrationElementSelected = null;
            ViewModel.CalibrationLineSelected = null;
            if (WpfPlotControl.SelectedPeak != null)
            {
                WpfPlotControl.ResetSelectedPeak(WpfPlotControl.SelectedPeak);
                WpfPlotControl.SelectedPeak = null;
            }

            if (ViewModel.Calibration.CalibrationPoints.Count >= 2 && CanCalibrate == false)
            {
                MessageBox.Show("Zadané hodnoty kalibračných bodov nie sú priamo úmerné.", "Calibration", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ViewModel.OnPropertyChanged();
        }

        private bool CanCalibrate
        {
            get
            {
                if (ViewModel.Calibration.CalibrationPoints.Count < 2) return false;
                List<CalibrationPoint> orderedPoints = ViewModel.Calibration.CalibrationPoints.OrderBy(point => point.Channel).ToList();
                for (int i = 0; i < orderedPoints.Count - 1; i++)
                {
                    if (orderedPoints[i].ElementLine?.Energy >= orderedPoints[i + 1].ElementLine?.Energy) return false;
                }
                return true;
            }
        }

        private void Button_DeleteCalibrationPoint_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CalibrationPointSelected != null) ViewModel.Calibration.CalibrationPoints.Remove(ViewModel.CalibrationPointSelected);
        }

        private void Button_CancelCalibrationPoint_Click(object sender, RoutedEventArgs e)
        {
            DataGridCalibrationPoints.UnselectAll();
        }

        private void Button_Calibrate_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Calibration.CalibrationPoints.Count >= 2 && CanCalibrate == false)
            {
                MessageBox.Show("Zadané hodnoty kalibračných bodov nie sú priamo úmerné.", "Calibration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (MainModel.Data.ReferencePoints is null)
            {
                MessageBox.Show("Chybaju referencne body.", "Calibration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CalibrationResponse? calibrationResponse = PythonService.Instance.GetData<CalibrationProperties, CalibrationResponse>(new CalibrationProperties()
            {
                DataToPredict = MainModel.Data.ReferencePoints.Select(point => point.Channel).ToList(),
                Channels = ViewModel.Calibration.CalibrationPoints.Select(point => point.Channel).ToList(),
                Energies = ViewModel.Calibration.CalibrationPoints.Select(point => point.ElementLine?.Energy).Cast<double>().ToList()
            });
            if (calibrationResponse != null)
            {
                MainModel.Data.Calibration = new()
                {
                    Intercept = Math.Round(calibrationResponse.Intercept, 3),
                    InterceptStderr = Math.Round(calibrationResponse.InterceptStderr, 3),
                    Pvalue = Math.Round(calibrationResponse.Pvalue, 3),
                    Rvalue = Math.Round(calibrationResponse.Rvalue, 3),
                    Slope = Math.Round(calibrationResponse.Slope, 3),
                    Stderr = Math.Round(calibrationResponse.Stderr, 3),
                    CalibrationPoints = ViewModel.Calibration.CalibrationPoints
                };
                ViewModel.Calibration = MainModel.Data.Calibration;
            }
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(CalibrationView));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
