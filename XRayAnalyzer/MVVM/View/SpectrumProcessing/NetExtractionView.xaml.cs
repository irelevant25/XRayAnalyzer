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
using XRayAnalyzer.MVVM.Model.Communication.Calibration;
using XRayAnalyzer.MVVM.Model.Communication.NetExtraction;
using XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    /// <summary>
    /// Interaction logic for NetExtractionView.xaml
    /// </summary>
    public partial class NetExtractionView : UserControl
    {
        public NetExtractionViewModel ViewModel { get; set; } = new NetExtractionViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public NetExtractionView()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (WpfPlotControl is null) return;

            if ((bool)e.NewValue)
            {
                // To visible
                if (MainModel.Data.ReferencePeaks is not null)
                {
                    ViewModel.Peaks = new(MainModel.Data.ReferencePeaks);
                    ViewModel.OnPropertyChanged();
                }
            }
            else
            {
                //To not visible
            }
        }

        private void Button_GetAreas_Click(object sender, RoutedEventArgs e)
        {
            if (MainModel.Data.ReferencePeaks is null || MainModel.Data.ReferencePoints is null) return;
            if (MainModel.Data.Background is null)
            {
                foreach (Peak peak in MainModel.Data.ReferencePeaks)
                {
                    List<SignalPoint> peakPoints = MainModel.Data.ReferencePoints.Where(point => point.Channel >= peak.LeftBaseSignalPoint?.Channel && point.Channel <= peak.RightBaseSignalPoint?.Channel).ToList();
                    GrossAreaTrapezoidalResponse? grossAreaTrapezoidalResponse = PythonService.Instance.GetData<GrossAreaTrapezoidalProperties, GrossAreaTrapezoidalResponse>(new GrossAreaTrapezoidalProperties()
                    {
                        DataX = peakPoints.Select(x => (double)x.Channel).ToList(),
                        DataY = peakPoints.Select(x => x.Value).ToList(),
                    });
                    NetAreaTrapezoidalResponse? netAreaTrapezoidalResponse = PythonService.Instance.GetData<NetAreaTrapezoidalProperties, NetAreaTrapezoidalResponse>(new NetAreaTrapezoidalProperties()
                    {
                        DataX = peakPoints.Select(x => (double)x.Channel).ToList(),
                        DataY = peakPoints.Select(x => x.Value).ToList(),
                    });
                    if (grossAreaTrapezoidalResponse is not null && netAreaTrapezoidalResponse is not null)
                    {
                        peak.GrossArea = (int?)grossAreaTrapezoidalResponse.Data?.First();
                        peak.NetArea = (int?)netAreaTrapezoidalResponse.Data?.First();
                    }
                }
            }
            else
            {
                foreach (Peak peak in MainModel.Data.ReferencePeaks)
                {
                    List<SignalPoint> peakPoints = MainModel.Data.ReferencePoints.Where(point => point.Channel >= peak.LeftBaseSignalPoint?.Channel && point.Channel <= peak.RightBaseSignalPoint?.Channel).ToList();
                    List<double> dataY = new();
                    foreach(SignalPoint point in peakPoints)
                    {
                        dataY.Add(point.Value - MainModel.Data.Background.First(x => x.Channel == point.Channel).Value);
                    }
                    GrossAreaTrapezoidalResponse? grossAreaTrapezoidalResponse = PythonService.Instance.GetData<GrossAreaTrapezoidalProperties, GrossAreaTrapezoidalResponse>(new GrossAreaTrapezoidalProperties()
                    {
                        DataX = peakPoints.Select(x => (double)x.Channel).ToList(),
                        DataY = dataY,
                    });
                    if (grossAreaTrapezoidalResponse is not null)
                    {
                        peak.GrossArea = (int?)grossAreaTrapezoidalResponse.Data?.First();
                        peak.NetArea = (int?)grossAreaTrapezoidalResponse.Data?.First();
                    }
                }
            }

            ViewModel.Peaks = new(MainModel.Data.ReferencePeaks);
            ViewModel.OnPropertyChanged();
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(NetExtractionView));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
