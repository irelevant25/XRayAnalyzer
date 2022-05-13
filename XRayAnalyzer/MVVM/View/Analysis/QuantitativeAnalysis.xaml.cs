using System.Linq;
using System.Windows;
using System.Windows.Controls;
using XRayAnalyzer.Controls;
using XRayAnalyzer.MVVM.Model.Communication.Analysis;
using XRayAnalyzer.MVVM.ViewModel.Analysis;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.MVVM.View.Analysis
{
    /// <summary>
    /// Interaction logic for QuantitativeAnalysis.xaml
    /// </summary>
    public partial class QuantitativeAnalysis : UserControl
    {
        public QuantitativeAnalysisViewModel ViewModel { get; set; } = new QuantitativeAnalysisViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public QuantitativeAnalysis()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (Dataset.ElementsInfo is null) return;
            ViewModel.ElementsInfo = new(Dataset.ElementsInfo);

            if (Dataset.DetectorEfficiencies is null) return;
            ViewModel.DetectorEfficiencies = new(Dataset.DetectorEfficiencies);
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
            }
        }

        private async void Button_QuantitativeAnalysisRun_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AnalysisRunning = true;
            if (MainModel.Data.ReferencePeaks is null || ViewModel.QuantitativeAnalysisProperties is null || MainModel.Data.ReferencePeaks.Count == 0 || Dataset.ElementsLines is null) return;
            MainModel.Data.QuantitativeAnalysisProperties = new QuantitativeAnalysisProperties()
            {
                XrayMassCoefficients = Dataset.XrayMassCoefficients?.ToList(),
                DetectorEfficiencies = ViewModel.QuantitativeAnalysisProperties.DetectorEfficiency,
                FluorescentYields = Dataset.FluorescentYields?.ToList(),
                JumpRatios = Dataset.JumpRatios?.ToList(),
                PrimaryElement = ViewModel.QuantitativeAnalysisProperties.PrimaryElement?.Number,
                Elements = MainModel.Data.ReferencePeaks.Select(peak => peak.ElementLine?.Element).Cast<int>().ToList(),
                ElementsEnergies = MainModel.Data.ReferencePeaks.Select(peak => peak.HighestSignalPoint.Energy).ToList(),
                ElementsAreas = MainModel.Data.ReferencePeaks.Select(peak => peak.NetArea).Cast<int>().ToList(),
                ElementsRadrates = MainModel.Data.ReferencePeaks.Select(peak => peak.ElementLine?.Rate).Cast<double>().ToList(),
                ElementsLines = MainModel.Data.ReferencePeaks.Select(peak => peak.ElementLine?.Line).Cast<string>().ToList(),
                XrayTubeAngel = ViewModel.QuantitativeAnalysisProperties.XRayTubeSampleAngle,
                DetectorAngel = ViewModel.QuantitativeAnalysisProperties.DetectorSampleAngle
            };
            QuantitativeAnalysisResponse? quantitativeAnalysisResponse = await PythonService.Instance.GetDataAsync<QuantitativeAnalysisProperties, QuantitativeAnalysisResponse>(MainModel.Data.QuantitativeAnalysisProperties);

            if (quantitativeAnalysisResponse is null) return;
            if (quantitativeAnalysisResponse.Data is not null)
            {
                MainModel.Data.QuantitativeAnalysis = quantitativeAnalysisResponse.Data;
                MainModel.Data.QuantitativeAnalysis.ForEach(quantitativeAnalysisItem =>
                {
                    quantitativeAnalysisItem.Peak = MainModel.Data.ReferencePeaks.Find(peak => peak.HighestSignalPoint.Energy == quantitativeAnalysisItem.Energy);
                });
                ViewModel.QuantitativeAnalysisItems = new(MainModel.Data.QuantitativeAnalysis);
            }
            ViewModel.AnalysisRunning = false;
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(QuantitativeAnalysis));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
