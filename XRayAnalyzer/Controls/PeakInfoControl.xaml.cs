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
using XRayAnalyzer.MVVM.Model;

namespace XRayAnalyzer.Controls
{
    /// <summary>
    /// Interaction logic for PeakInfoControl.xaml
    /// </summary>
    public partial class PeakInfoControl : UserControl
    {
        public static readonly DependencyProperty SelectedPeak_Property = DependencyProperty.Register(nameof(SelectedPeak), typeof(Peak), typeof(PeakInfoControl), new FrameworkPropertyMetadata(null));
        public Peak SelectedPeak
        {
            get
            {
                return (Peak)GetValue(SelectedPeak_Property);
            }
            set
            {
                SetValue(SelectedPeak_Property, value);
            }
        }

        public PeakInfoControl()
        {
            InitializeComponent();
        }
    }
}
