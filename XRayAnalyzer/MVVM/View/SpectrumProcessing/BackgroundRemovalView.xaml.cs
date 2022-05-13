using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XRayAnalyzer.Controls;
using XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;
using ScottPlot.Plottable;
using System.Drawing;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.Model.Communication.BackgroundRemoval;
using XRayAnalyzer.Objects.Enums;

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    /// <summary>
    /// Interaction logic for BackgroundRemoval.xaml
    /// </summary>
    public partial class BackgroundRemovalView : UserControl
    {
        public BackgroundRemovalViewModel ViewModel { get; set; } = new BackgroundRemovalViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public BackgroundRemovalView()
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
                WpfPlotControl.CanSelectPeak = true;
            }
            else
            {
                //To not visible
                WpfPlotControl.ComboBox_XAxisValue_IsEnabled = false;
                WpfPlotControl.ComboBox_YAxisValue_IsEnabled = false;
                WpfPlotControl.CanSelectPeak = false;

                BindingOperations.ClearAllBindings(WpfPlotControl);
                WpfPlotControl?.ResetSelectedPeak();
            }
        }

        private void Slider_BackgroundRemovalProperty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ViewModel.BackgroundRemovalLivePreview == true)
            {
                BackgroundRemovalPreview();
            }
        }

        private void Button_BackgroundRemovalPreview_Click(object sender, RoutedEventArgs e)
        {
            BackgroundRemovalPreview();
        }

        private void Button_BackgroundRemovalConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.BackgroundPoints is null || MainModel.Data.ReferencePoints is null) return;
            MainModel.Data.Background = new();
            for (int index = 0; index < ViewModel.BackgroundPoints.Count; index++)
            {
                SignalPoint newPoint = new ()
                {
                    Channel = MainModel.Data.ReferencePoints[index].Channel,
                    Value = (int)ViewModel.BackgroundPoints[index].Value
                };
                if (newPoint.Value < 0) newPoint.Value = 0;
                MainModel.Data.Background.Add(newPoint);
            }

            if (ViewModel.BackgroundPlot is not null)
            {
                ((SignalPlotXY)ViewModel.BackgroundPlot).UpdatePlotColor(PlotColorType.Background);
                WpfPlotControl.PlotRefresh();
            }
            ViewModel.BackgroundPlot = null;
        }

        private void Button_BackgroundRemovalCancel_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.BackgroundPlot is null) return;
            WpfPlotControl.PlotRemoveWithRefresh(ViewModel.BackgroundPlot);
            ViewModel.BackgroundPlot = null;
        }

        private async void BackgroundRemovalPreview()
        {
            if (MainModel.Data.ReferencePoints is null || ViewModel.BackgroundRemovalProperties is null || MainModel.Data.ReferencePoints.Count == 0) return;
            ViewModel.BackgroundRemovalProperties.Points = MainModel.Data.ReferencePoints.Select(x => x.Value).ToList();
            BackgroundRemovalZhangfitResponse? backgroundRemovalResponse = await PythonService.Instance.GetDataAsync<BackgroundRemovalZhangfitProperties, BackgroundRemovalZhangfitResponse>(new BackgroundRemovalZhangfitProperties()
            {
                Data = ViewModel.BackgroundRemovalProperties.Points,
                Itermax = ViewModel.BackgroundRemovalProperties.Itermax,
                Lambda = ViewModel.BackgroundRemovalProperties.Lambda
            });

            if (ViewModel.BackgroundPlot is not null) WpfPlotControl.PlotRemove(ViewModel.BackgroundPlot);
            if (backgroundRemovalResponse is null) return;
            ViewModel.BackgroundPlot = null;

            if (backgroundRemovalResponse.Data is not null)
            {
                ViewModel.BackgroundPoints = new();
                for (int index = 0; index < backgroundRemovalResponse.Data.Count; index++)
                {
                    SignalPoint point = new()
                    {
                        Channel = MainModel.Data.ReferencePoints[index].Channel,
                        Value = backgroundRemovalResponse.Data[index]
                    };
                    ViewModel.BackgroundPoints.Add(point);
                }
                ViewModel.BackgroundPlot = WpfPlotControl.PlotBackground(ViewModel.BackgroundPoints.ToList());
            }
            WpfPlotControl.PlotRefresh();
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(BackgroundRemovalView));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
