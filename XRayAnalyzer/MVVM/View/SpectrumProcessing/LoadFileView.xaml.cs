using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using XRayAnalyzer.Controls;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.Communication.Calibration;
using XRayAnalyzer.MVVM.Model.Communication.MCA;
using XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    /// <summary>
    /// Interaction logic for LoadFile.xaml
    /// </summary>
    public partial class LoadFileView : UserControl
    {
        public LoadFileViewModel ViewModel { get; set; } = new LoadFileViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public LoadFileView()
        {
            InitializeComponent();

            // for testing purpose
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (WpfPlotControl is null) return;
            
            if ((bool)e.NewValue)
            {
                // To visible
                WpfPlotControl.SetBinding(PlotControl.ComboBox_XAxisValue_IsEnabled_Property, new Binding(nameof(ViewModel.CanChangeXAxis)) { Source = ViewModel });
                WpfPlotControl.SetBinding(PlotControl.ComboBox_YAxisValue_IsEnabled_Property, new Binding(nameof(ViewModel.CanChangeYAxis)) { Source = ViewModel });
                WpfPlotControl.SetBinding(PlotControl.CanSelectPeak_Property, new Binding(nameof(ViewModel.CanSelectPeak)) { Source = ViewModel });
            }
            else
            {
                //To not visible
                BindingOperations.ClearAllBindings(WpfPlotControl);
                WpfPlotControl?.ResetSelectedPeak();
            }
        }

        private void LoadFile(string fullFileName)
        {
            string extension = Path.GetExtension(fullFileName);

            if (extension.ToLower() == ".json")
            {
                string fileData;
                using (StreamReader r = new StreamReader(fullFileName))
                {
                    fileData = r.ReadToEnd();
                }

                JSON? jsonData = fileData.JsonDeserialize<JSON>();
                if (jsonData?.ReferencePoints is null)
                {
                    MessageBox.Show("Unable to load data.", "File load", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MainModel.Data = jsonData;

                    //LogBase
                    MainModel.Data.LogBase = Math.Pow(jsonData.ReferencePoints.Select(x => x.Value).Max(), 1.0 / (jsonData.ReferencePoints.Select(x => x.Value).Max() * 1.05));

                    ViewModel.Gain = Convert.ToInt32(jsonData.MCAFile?.Spectrum?["GAIN"]);
                    ViewModel.PeaksCount = MainModel.Data.ReferencePeaks?.Count;
                    ViewModel.Calibrated = MainModel.Data.Calibration is not null;
                    ViewModel.BackgroundRemoved = false;
                    ViewModel.SumPeaksRemoved = false;
                    ViewModel.Smoothed = false;
                    ViewModel.NetExtracted = false;
                    ViewModel.QualitativeAnalysis = false;
                    ViewModel.QuantitativeAnalysis = false;

                    ViewModel.CanChangeXAxis = Helper.CanChangeX();
                    ViewModel.CanChangeYAxis = Helper.CanChangeX();

                    WpfPlotControl.PlotClear();
                    WpfPlotControl.PlotSpectrum(MainModel.Data.ReferencePoints);
                    WpfPlotControl.PlotPeaks(MainModel.Data.ReferencePeaks);
                }
            }
            else if (extension.ToLower() == ".mca")
            {
                MCAResponse? mcaData = PythonService.Instance.GetData<MCAProperties, MCAResponse>(new MCAProperties() { FileName = fullFileName });
                if (mcaData?.Data is null)
                {
                    MessageBox.Show("Unable to load data.", "File load", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //LogBase
                MainModel.Data.LogBase = Math.Pow(mcaData.Data.Max(), 1.0 / (mcaData.Data.Max() * 1.05));

                //ReferencePoints
                MainModel.Data.ReferencePoints = new ();
                for (int index = 1; index <= mcaData.Data.Count; index++)
                {
                    double value = mcaData.Data[index - 1];

                    MainModel.Data.ReferencePoints.Add(new SignalPoint()
                    {
                        Value = (int)value,
                        Channel = index
                    });
                }

                //ReferencePeaks
                if (mcaData.PeaksStart is not null && mcaData.PeaksEnd is not null)
                {
                    MainModel.Data.ReferencePeaks = new();
                    for (int index = 0; index < mcaData.PeaksStart.Count; index++)
                    {
                        Peak peak = new ()
                        {
                            Id = index,
                            LeftBaseSignalPoint = MainModel.Data.ReferencePoints[mcaData.PeaksStart[index]],
                            RightBaseSignalPoint = MainModel.Data.ReferencePoints[mcaData.PeaksEnd[index]],
                        };
                        peak.HighestSignalPoint = MainModel.Data.ReferencePoints
                            .Skip(peak.LeftBaseSignalPoint.Channel)
                            .Take(peak.RightBaseSignalPoint.Channel - peak.LeftBaseSignalPoint.Channel + 1)
                            .OrderByDescending(x => x.Value)
                            .First();
                        MainModel.Data.ReferencePeaks.Add(peak);
                    }
                }

                //Calibration
                if (mcaData.CalibrationChannels is not null && mcaData.CalibrationEnergies is not null)
                {
                    CalibrationResponse? calibrationResponse = PythonService.Instance.GetData<CalibrationProperties, CalibrationResponse>(new CalibrationProperties()
                    {
                        DataToPredict = MainModel.Data.ReferencePoints.Select(point => point.Channel).ToList(),
                        Channels = mcaData.CalibrationChannels,
                        Energies = mcaData.CalibrationEnergies
                    });
                    if (calibrationResponse != null)
                    {
                        MainModel.Data.Calibration = new Calibration()
                        {
                            Intercept = Math.Round(calibrationResponse.Intercept, 3),
                            InterceptStderr = Math.Round(calibrationResponse.InterceptStderr, 3),
                            Pvalue = Math.Round(calibrationResponse.Pvalue, 3),
                            Rvalue = Math.Round(calibrationResponse.Rvalue, 3),
                            Slope = Math.Round(calibrationResponse.Slope, 3),
                            Stderr = Math.Round(calibrationResponse.Stderr, 3),
                        };
                        MainModel.Data.Calibration.CalibrationPoints = new();
                        for (int index = 0; index < mcaData.CalibrationChannels.Count; index++)
                        {
                            CalibrationPoint calibrationPoint = new()
                            {
                                Channel = mcaData.CalibrationChannels[index],
                                ElementLine = new()
                                {
                                    Energy = mcaData.CalibrationEnergies[index]
                                }
                            };
                            MainModel.Data.Calibration.CalibrationPoints.Add(calibrationPoint);
                        }
                    }
                }

                ViewModel.Gain = Convert.ToInt32(mcaData.Spectrum?["GAIN"]);
                ViewModel.PeaksCount = MainModel.Data.ReferencePeaks?.Count;
                ViewModel.Calibrated = MainModel.Data.Calibration is not null;
                ViewModel.BackgroundRemoved = false;
                ViewModel.SumPeaksRemoved = false;
                ViewModel.Smoothed = false;
                ViewModel.NetExtracted = false;
                ViewModel.QualitativeAnalysis = false;
                ViewModel.QuantitativeAnalysis = false;

                ViewModel.CanChangeXAxis = Helper.CanChangeX();
                ViewModel.CanChangeYAxis = Helper.CanChangeX();

                WpfPlotControl.PlotClear();
                WpfPlotControl.PlotSpectrum(MainModel.Data.ReferencePoints);
                WpfPlotControl.PlotPeaks(MainModel.Data.ReferencePeaks);
            }
        }

        private void Button_SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "(*.mca, *.json)|*.mca;*.json|*.mca|*.mca|*.json|*.json"
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                ViewModel.SelectedFile = dialog.FileName;
                LoadFile(dialog.FileName);
            }
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(LoadFileView));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
