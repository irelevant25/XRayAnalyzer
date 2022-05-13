using Microsoft.Win32;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.MVVM.ViewModel.SpectrumProcessing;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Objects.Enums;
using XRayAnalyzer.Services;

namespace XRayAnalyzer.MVVM.View.SpectrumProcessing
{
    public partial class SpectrumProcessingView : UserControl
    {
        public SpectrumProcessingViewModel ViewModel { get; set; } = new SpectrumProcessingViewModel();

        public SpectrumProcessingView()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsLoadFileOperation = true;
        }
    }
}
