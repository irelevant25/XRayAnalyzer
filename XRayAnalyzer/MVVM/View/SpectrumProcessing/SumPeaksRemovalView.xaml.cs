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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XRayAnalyzer.Controls;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing;
using XRayAnalyzer.Objects;

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    /// <summary>
    /// Interaction logic for SumPeaksRemoval.xaml
    /// </summary>
    public partial class SumPeaksRemovalView : UserControl
    {
        public SumPeaksRemovalViewModel ViewModel { get; set; } = new SumPeaksRemovalViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public SumPeaksRemovalView()
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
                if (MainModel.Data.ReferencePeaks is not null)
                {
                    ViewModel.Peaks = new(MainModel.Data.ReferencePeaks);
                    if (MainModel.Data.SumPeaks is not null)
                    {
                        ViewModel.SumPeaks = new(MainModel.Data.SumPeaks);
                    }
                }
            }
            else
            {
                //To not visible
                WpfPlotControl.ComboBox_XAxisValue_IsEnabled = false;
                WpfPlotControl.ComboBox_YAxisValue_IsEnabled = false;
                WpfPlotControl.CanSelectPeak = false;
                WpfPlotControl?.ResetSelectedPeak();
            }
        }

        private void Button_ConfirmSumPeaks_Click(object sender, RoutedEventArgs e)
        {
            if (MainModel.Data.ReferencePeaks is not null)
            {
                MainModel.Data.SumPeaks = ViewModel.SumPeaks?.ToList();
                MainModel.Data.SumPeaks?.ForEach(peak => MainModel.Data.ReferencePeaks?.Remove(peak));
                ViewModel.SumPeaks = null;
                ViewModel.Peaks = new(MainModel.Data.ReferencePeaks);
                WpfPlotControl.PlotPeaks(MainModel.Data.ReferencePeaks);
            }
        }

        private void Button_DetectSumPeaks_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Peaks is null) return;
            ViewModel.SumPeaks = new();
            foreach (Peak potencialSumPeak in ViewModel.Peaks)
            {
                Peak? sumPeak = ViewModel.Peaks.Where(peak => peak.Id != potencialSumPeak.Id && peak.HighestSignalPoint?.Energy == 2 * potencialSumPeak.HighestSignalPoint?.Energy).FirstOrDefault();
                if (sumPeak is not null)
                {
                    ViewModel.SumPeaks.Add(sumPeak);
                }
            }
            ViewModel.OnPropertyChanged();
        }

        private void Button_CancelSumPeaks_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SumPeaks = null;
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(SumPeaksRemovalView));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
