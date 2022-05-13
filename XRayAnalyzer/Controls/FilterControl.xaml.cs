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
using XRayAnalyzer.MVVM.ViewModel.DataViewer;

namespace XRayAnalyzer.Controls
{
    /// <summary>
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl
    {
        public static readonly DependencyProperty Filter_Property = DependencyProperty.Register(nameof(Filter), typeof(FilterViewModel), typeof(FilterControl), new FrameworkPropertyMetadata(null));
        public FilterViewModel Filter
        {
            get
            {
                return (FilterViewModel)GetValue(Filter_Property);
            }
            set
            {
                SetValue(Filter_Property, value);
            }
        }

        public static readonly DependencyProperty Button_Submit_Click_Property = DependencyProperty.Register(nameof(Button_Submit_Click_Action), typeof(Action<object, RoutedEventArgs>), typeof(FilterControl), new FrameworkPropertyMetadata(null));
        public Action<object, RoutedEventArgs> Button_Submit_Click_Action
        {
            get
            {
                return (Action<object, RoutedEventArgs>)GetValue(Button_Submit_Click_Property);
            }
            set
            {
                SetValue(Button_Submit_Click_Property, value);
            }
        }

        public static readonly DependencyProperty Button_Reset_Click_Property = DependencyProperty.Register(nameof(Button_Reset_Click_Action), typeof(Action<object, RoutedEventArgs>), typeof(FilterControl), new FrameworkPropertyMetadata(null));
        public Action<object, RoutedEventArgs> Button_Reset_Click_Action
        {
            get
            {
                return (Action<object, RoutedEventArgs>)GetValue(Button_Reset_Click_Property);
            }
            set
            {
                SetValue(Button_Reset_Click_Property, value);
            }
        }

        public FilterControl()
        {
            InitializeComponent();
        }

        private void Button_Submit_Click(object sender, RoutedEventArgs e)
        {
            Button_Submit_Click_Action?.Invoke(sender, e);
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            Button_Reset_Click_Action?.Invoke(sender, e);
        }
    }
}
