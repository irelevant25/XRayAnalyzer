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
using XRayAnalyzer.MVVM.Model.Communication.Analysis;
using XRayAnalyzer.MVVM.ViewModel.Analysis;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.MVVM.View.Analysis
{
    /// <summary>
    /// Interaction logic for QualitativeAnalysis.xaml
    /// </summary>
    public partial class QualitativeAnalysis : UserControl
    {
        public QualitativeAnalysisViewModel ViewModel { get; set; } = new QualitativeAnalysisViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public QualitativeAnalysis()
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
                WpfPlotControl.SelectedPeak_Changed_Action += SelectedPeak_Changed;
                WpfPlotControl.PlotElementMarkers();
            }
            else
            {
                //To not visible
                WpfPlotControl.ComboBox_XAxisValue_IsEnabled = false;
                WpfPlotControl.ComboBox_YAxisValue_IsEnabled = false;
                WpfPlotControl.CanSelectPeak = false;
                WpfPlotControl.SelectedPeak_Changed_Action -= SelectedPeak_Changed;
                ViewModel.SelectedQualitativeAnalysisItem = null;
            }
        }

        private void SelectedPeak_Changed(Peak? peak)
        {
            ViewModel.SelectedQualitativeAnalysisItem = MainModel.Data.QualitativeAnalysis?.Find(x => x.Energy == peak?.HighestSignalPoint.Energy);
        }

        private async void Button_QualitativeAnalysisRun_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AnalysisRunning = true;
            if (MainModel.Data.ReferencePeaks is null || ViewModel.QualitativeAnalysisProperties is null || MainModel.Data.ReferencePeaks.Count == 0 || Dataset.ElementsLines is null) return;
            MainModel.Data.QualitativeAnalysisProperties = new QualitativeAnalysisProperties()
            {
                ElementsLines = Dataset.ElementsLines.ToList(),
                Energies = MainModel.Data.ReferencePeaks.Select(peak => peak.HighestSignalPoint.Energy).ToList(),
                EnergyAbsTreshold = ViewModel.QualitativeAnalysisProperties.EnergyAbsTreshold,
                RateTreshold = ViewModel.QualitativeAnalysisProperties.RateTreshold
            };
            QualitativeAnalysisResponse? qualitativeAnalysisResponse = await PythonService.Instance.GetDataAsync<QualitativeAnalysisProperties, QualitativeAnalysisResponse>(MainModel.Data.QualitativeAnalysisProperties);

            if (qualitativeAnalysisResponse is null) return;
            if (qualitativeAnalysisResponse.Data is not null)
            {
                MainModel.Data.QualitativeAnalysis = qualitativeAnalysisResponse.Data;
                MainModel.Data.QualitativeAnalysis.ForEach(qualitativeAnalysisItem =>
                {
                    qualitativeAnalysisItem.BestMatches?.ForEach(elementLine => elementLine.IncludeElementInfo());
                    qualitativeAnalysisItem.PossibleMatches?.ForEach(elementLine => elementLine.IncludeElementInfo());

                    Peak? peak = MainModel.Data.ReferencePeaks.Find(peak => peak.HighestSignalPoint.Energy == qualitativeAnalysisItem.Energy);
                    if (peak is not null)
                    {
                        peak.ElementLine = qualitativeAnalysisItem.BestMatches?.LastOrDefault();
                    }
                });
                WpfPlotControl.PlotElementMarkers();
            }
            ViewModel.AnalysisRunning = false;
        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(QualitativeAnalysis));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
