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

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    /// <summary>
    /// Interaction logic for SmoothingView.xaml
    /// </summary>
    public partial class SmoothingView : UserControl
    {
        public SmoothingView()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        /* PROPERTIES */

        public static readonly DependencyProperty WpfPlotControl_Property = DependencyProperty.Register(nameof(WpfPlotControl), typeof(PlotControl), typeof(SmoothingView));
        public PlotControl WpfPlotControl
        {
            get { return (PlotControl)GetValue(WpfPlotControl_Property); }
            set { SetValue(WpfPlotControl_Property, value); }
        }
    }
}
