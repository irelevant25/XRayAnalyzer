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
using XRayAnalyzer.MVVM.ViewModel.Analysis;
using XRayAnalyzer.Objects;

namespace XRayAnalyzer.MVVM.View.Analysis
{
    /// <summary>
    /// Interaction logic for AnalysisView.xaml
    /// </summary>
    public partial class AnalysisView : UserControl
    {
        private AnalysisViewModel ViewModel { get; set; } = new AnalysisViewModel();
        private MainModel MainModel { get; set; } = MainModel.Instance;

        public AnalysisView()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsQualitativeAnalysisOperation = true;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (WpfPlotControl is null) return;

            if ((bool)e.NewValue)
            {
                // To visible
                WpfPlotControl.PlotClear();
                WpfPlotControl.PlotSpectrum(MainModel.Data.ReferencePoints);
                WpfPlotControl.PlotPeaks(MainModel.Data.ReferencePeaks);
            }
            else
            {
                //To not visible
            }
        }
    }
}
