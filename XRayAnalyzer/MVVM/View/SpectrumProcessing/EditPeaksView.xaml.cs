using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using XRayAnalyzer.Controls;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.Communication.PeakSearch;
using XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    /// <summary>
    /// Interaction logic for EditPeaksView.xaml
    /// </summary>
    public partial class EditPeaksView : UserControl
    {
        public EditPeaksViewModel ViewModel { get; set; } = new EditPeaksViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public EditPeaksView()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (WpfPlotControl is null) return;
            
            if ((bool)e.NewValue)
            {
                // To visible
                WpfPlotControl.ComboBox_XAxisValue_IsEnabled = Helper.CanChangeX();
                WpfPlotControl.ComboBox_YAxisValue_IsEnabled = Helper.CanChangeY();
                WpfPlotControl.SetBinding(PlotControl.CanAddPeak_Property, new Binding(nameof(ViewModel.IsAddingPeak)) { Source = ViewModel });
                WpfPlotControl.SetBinding(PlotControl.CanEditPeak_Property, new Binding(nameof(ViewModel.IsEditingPeak)) { Source = ViewModel });
                WpfPlotControl.SetBinding(PlotControl.CanRemovePeak_Property, new Binding(nameof(ViewModel.IsRemovingPeak)) { Source = ViewModel });
                WpfPlotControl.SetBinding(PlotControl.CanSelectPeak_Property, new Binding(nameof(ViewModel.IsSelectOnlyPeak)) { Source = ViewModel });
                WpfPlotControl.Wpfplot_MouseLeftButtonDown_Action += Wpfplot_MouseLeftButtonDown;
            }
            else
            {
                //To not visible
                WpfPlotControl.ComboBox_XAxisValue_IsEnabled = false;
                WpfPlotControl.ComboBox_YAxisValue_IsEnabled = false;
                WpfPlotControl.Wpfplot_MouseLeftButtonDown_Action -= Wpfplot_MouseLeftButtonDown;
                BindingOperations.ClearAllBindings(WpfPlotControl);

                if (ViewModel.IsSelectOnlyPeak) WpfPlotControl.ResetSelectedPeak(WpfPlotControl.OriginalSelectedPeak);
                else if (ViewModel.IsEditingPeak) WpfPlotControl.ResetSelectedPeak(WpfPlotControl.OriginalSelectedPeak);
                else if (ViewModel.IsAddingPeak)
                {
                    WpfPlotControl.PlotRemoveWithRefresh(WpfPlotControl.SelectedPeak?.SignalPlot);
                    WpfPlotControl.SelectedPeak = null;
                }
            }
        }

        private void Wpfplot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e, Peak? peak)
        {
            if (ViewModel.IsRemovingPeak && peak is not null)
            {
                MainModel.Data.ReferencePeaks?.Remove(peak);
            }
        }

        private void Button_ConfirmNewPeak_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsAddingPeak && WpfPlotControl.SelectedPeak?.SignalPlot != null)
            {
                WpfPlotControl.Peaks?.Add(WpfPlotControl.SelectedPeak);
                MainModel.Data.ReferencePeaks?.Add(WpfPlotControl.SelectedPeak);
                WpfPlotControl.SelectedPeak.SignalPlot.UpdatePlotColor();
                WpfPlotControl.SelectedPeak = null;
                WpfPlotControl.PlotRefresh();
            }
        }

        private void Button_CancelNewPeak_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsAddingPeak && WpfPlotControl.SelectedPeak?.SignalPlot != null)
            {
                WpfPlotControl.PlotRemoveWithRefresh(WpfPlotControl.SelectedPeak.SignalPlot);
                WpfPlotControl.SelectedPeak = null;
            }
        }

        private void Button_ConfirmEditedPeak_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsEditingPeak && WpfPlotControl.SelectedPeak?.SignalPlot != null)
            {
                WpfPlotControl.SelectedPeak.SignalPlot.UpdatePlotColor();
                WpfPlotControl.SelectedPeak = null;
                WpfPlotControl.PlotRefresh();
            }
        }

        private void Button_CancelEditedPeak_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsEditingPeak)
            {
                WpfPlotControl.ResetSelectedPeak(WpfPlotControl.OriginalSelectedPeak);
            }
        }

        private void Button_SearchPickPreview_Click(object sender, RoutedEventArgs e)
        {
            FindAndPreviewPeaks();
        }

        private void Button_SearchPeaksConfirm_Click(object sender, RoutedEventArgs e)
        {
            MainModel.Data.ReferencePeaks = ViewModel.PeakSearch.Peaks;
            ViewModel.PeakSearch.Peaks?.ForEach(peak => peak.SignalPlot?.UpdatePlotColor());
            WpfPlotControl.PlotRefresh();
            ViewModel.PeakSearch.Peaks = null;
            ViewModel.OnPropertyChanged();

        }

        private void Button_SearchPeaksCancel_Click(object sender, RoutedEventArgs e)
        {
            WpfPlotControl.PlotPeaks(MainModel.Data.ReferencePeaks);
            ViewModel.PeakSearch.Peaks = null;
            ViewModel.OnPropertyChanged();
        }

        private void Slider_FindPeaksProperty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ViewModel.PeakSearchLivePreview == true)
            {
                FindAndPreviewPeaks();
            }
        }

        private async void FindAndPreviewPeaks()
        {
            if (MainModel.Data.ReferencePoints is null) return;

            if (MainModel.Data.ReferencePoints.Count > 0)
            {
                PeakSearchResponse? peakSearchResponse = await PythonService.Instance.GetDataAsync<PeakSearchProperties, PeakSearchResponse>(new PeakSearchProperties()
                {
                    Data = MainModel.Data.ReferencePoints.Select(point => (double)point.Value).ToList(),
                    Distance = ViewModel.PeakSearch.Distance,
                    Prominence = ViewModel.PeakSearch.Prominence,
                    Width = new () { ViewModel.PeakSearch.WidthFrom, ViewModel.PeakSearch.WidthTo },
                    Wlen = ViewModel.PeakSearch.Wlen
                });
                if (peakSearchResponse != null)
                {
                    ViewModel.PeakSearch.Peaks = new List<Peak>();
                    if (peakSearchResponse.PeaksIndexes != null && peakSearchResponse.LeftBasesIndexes != null && peakSearchResponse.RightBasesIndexes != null)
                    {
                        for (int i = 0; i < peakSearchResponse.PeaksIndexes.Count; i++)
                        {
                            SignalPlotXY initPeakPlot = new()
                            {
                                Color = (i % 2 == 0) ? Color.Blue : Color.Green
                            };
                            Peak newPeak = new()
                            {
                                Id = DateTime.Now.ToMiliseconds(),
                                HighestSignalPoint = MainModel.Data.ReferencePoints[peakSearchResponse.PeaksIndexes[i]],
                                LeftBaseSignalPoint = MainModel.Data.ReferencePoints[peakSearchResponse.LeftBasesIndexes[i]],
                                RightBaseSignalPoint = MainModel.Data.ReferencePoints[peakSearchResponse.RightBasesIndexes[i]],
                                SignalPlot = initPeakPlot
                            };
                            ViewModel.PeakSearch.Peaks.Add(newPeak);
                        }
                        WpfPlotControl.PlotPeaks(ViewModel.PeakSearch.Peaks);
                        ViewModel.OnPropertyChanged();
                    }
                }
            }
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(EditPeaksView));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
