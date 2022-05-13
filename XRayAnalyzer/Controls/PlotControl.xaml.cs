using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.Objects;
using XRayAnalyzer.Objects.Enums;

namespace XRayAnalyzer.Controls
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl
    {
        public static readonly DependencyProperty ComboBox_XAxisValue_SelectionChanged_Property = DependencyProperty.Register(nameof(ComboBox_XAxisValue_SelectionChanged_Action), typeof(Action<object, RoutedEventArgs>), typeof(PlotControl), new FrameworkPropertyMetadata(null));
        public Action<object, RoutedEventArgs> ComboBox_XAxisValue_SelectionChanged_Action
        {
            get
            {
                return (Action<object, RoutedEventArgs>)GetValue(ComboBox_XAxisValue_SelectionChanged_Property);
            }
            set
            {
                SetValue(ComboBox_XAxisValue_SelectionChanged_Property, value);
            }
        }

        public static readonly DependencyProperty ComboBox_YAxisValue_SelectionChanged_Property = DependencyProperty.Register(nameof(ComboBox_YAxisValue_SelectionChanged_Action), typeof(Action<object, RoutedEventArgs>), typeof(PlotControl), new FrameworkPropertyMetadata(null));
        public Action<object, RoutedEventArgs> ComboBox_YAxisValue_SelectionChanged_Action
        {
            get
            {
                return (Action<object, RoutedEventArgs>)GetValue(ComboBox_YAxisValue_SelectionChanged_Property);
            }
            set
            {
                SetValue(ComboBox_YAxisValue_SelectionChanged_Property, value);
            }
        }

        public static readonly DependencyProperty ComboBox_XAxisValue_IsEnabled_Property = DependencyProperty.Register(nameof(ComboBox_XAxisValue_IsEnabled), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool ComboBox_XAxisValue_IsEnabled
        {
            get
            {
                return (bool)GetValue(ComboBox_XAxisValue_IsEnabled_Property);
            }
            set
            {
                SetValue(ComboBox_XAxisValue_IsEnabled_Property, value);
            }
        }

        public static readonly DependencyProperty ComboBox_YAxisValue_IsEnabled_Property = DependencyProperty.Register(nameof(ComboBox_YAxisValue_IsEnabled), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool ComboBox_YAxisValue_IsEnabled
        {
            get
            {
                return (bool)GetValue(ComboBox_YAxisValue_IsEnabled_Property);
            }
            set
            {
                SetValue(ComboBox_YAxisValue_IsEnabled_Property, value);
            }
        }

        public static readonly DependencyProperty Wpfplot_MouseLeftButtonDown_Property = DependencyProperty.Register(nameof(Wpfplot_MouseLeftButtonDown_Action), typeof(Action<object, MouseButtonEventArgs, Peak?>), typeof(PlotControl), new FrameworkPropertyMetadata(null));
        public Action<object, MouseButtonEventArgs, Peak?>? Wpfplot_MouseLeftButtonDown_Action
        {
            get
            {
                return (Action<object, MouseButtonEventArgs, Peak?>)GetValue(Wpfplot_MouseLeftButtonDown_Property);
            }
            set
            {
                SetValue(Wpfplot_MouseLeftButtonDown_Property, value);
            }
        }

        public static readonly DependencyProperty Wpfplot_MouseLeftButtonUp_Property = DependencyProperty.Register(nameof(Wpfplot_MouseLeftButtonUp_Action), typeof(Action<object, MouseButtonEventArgs>), typeof(PlotControl), new FrameworkPropertyMetadata(null));
        public Action<object, MouseButtonEventArgs> Wpfplot_MouseLeftButtonUp_Action
        {
            get
            {
                return (Action<object, MouseButtonEventArgs>)GetValue(Wpfplot_MouseLeftButtonUp_Property);
            }
            set
            {
                SetValue(Wpfplot_MouseLeftButtonUp_Property, value);
            }
        }

        public static readonly DependencyProperty Wpfplot_MouseMove_Property = DependencyProperty.Register(nameof(Wpfplot_MouseMove_Action), typeof(Action<object, MouseEventArgs>), typeof(PlotControl), new FrameworkPropertyMetadata(null));
        public Action<object, MouseEventArgs> Wpfplot_MouseMove_Action
        {
            get
            {
                return (Action<object, MouseEventArgs>)GetValue(Wpfplot_MouseMove_Property);
            }
            set
            {
                SetValue(Wpfplot_MouseMove_Property, value);
            }
        }

        public static readonly DependencyProperty LinearScale_IsSelected_Property = DependencyProperty.Register(nameof(LinearScale_IsSelected), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(true));
        public bool LinearScale_IsSelected
        {
            get
            {
                return (bool)GetValue(LinearScale_IsSelected_Property);
            }
            set
            {
                SetValue(LinearScale_IsSelected_Property, value);
            }
        }

        public static readonly DependencyProperty LogarithmicScale_IsSelected_Property = DependencyProperty.Register(nameof(LogarithmicScale_IsSelected), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool LogarithmicScale_IsSelected
        {
            get
            {
                return (bool)GetValue(LogarithmicScale_IsSelected_Property);
            }
            set
            {
                SetValue(LogarithmicScale_IsSelected_Property, value);
            }
        }

        public static readonly DependencyProperty XInChannels_IsSelected_Property = DependencyProperty.Register(nameof(XInChannels_IsSelected), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(true));
        public bool XInChannels_IsSelected
        {
            get
            {
                return (bool)GetValue(XInChannels_IsSelected_Property);
            }
            set
            {
                SetValue(XInChannels_IsSelected_Property, value);
            }
        }

        public static readonly DependencyProperty XInEnergies_IsSelected_Property = DependencyProperty.Register(nameof(XInEnergies_IsSelected), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool XInEnergies_IsSelected
        {
            get
            {
                return (bool)GetValue(XInEnergies_IsSelected_Property);
            }
            set
            {
                SetValue(XInEnergies_IsSelected_Property, value);
            }
        }

        public static readonly DependencyProperty CanSelectPeak_Property = DependencyProperty.Register(nameof(CanSelectPeak), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool CanSelectPeak
        {
            get
            {
                return (bool)GetValue(CanSelectPeak_Property);
            }
            set
            {
                SetValue(CanSelectPeak_Property, value);
            }
        }

        public static readonly DependencyProperty CanAddPeak_Property = DependencyProperty.Register(nameof(CanAddPeak), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool CanAddPeak
        {
            get
            {
                return (bool)GetValue(CanAddPeak_Property);
            }
            set
            {
                SetValue(CanAddPeak_Property, value);
            }
        }

        public static readonly DependencyProperty CanEditPeak_Property = DependencyProperty.Register(nameof(CanEditPeak), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool CanEditPeak
        {
            get
            {
                return (bool)GetValue(CanEditPeak_Property);
            }
            set
            {
                SetValue(CanEditPeak_Property, value);
            }
        }

        public static readonly DependencyProperty CanRemovePeak_Property = DependencyProperty.Register(nameof(CanRemovePeak), typeof(bool), typeof(PlotControl), new FrameworkPropertyMetadata(false));
        public bool CanRemovePeak
        {
            get
            {
                return (bool)GetValue(CanRemovePeak_Property);
            }
            set
            {
                SetValue(CanRemovePeak_Property, value);
            }
        }

        public static readonly DependencyProperty SelectedPeak_Property = DependencyProperty.Register(nameof(SelectedPeak), typeof(Peak), typeof(PlotControl), new FrameworkPropertyMetadata(null));
        public Peak? SelectedPeak
        {
            get
            {
                return (Peak?)GetValue(SelectedPeak_Property);
            }
            set
            {
                SetValue(SelectedPeak_Property, value);
                SelectedPeak_Changed_Action?.Invoke(SelectedPeak);
            }
        }

        public static readonly DependencyProperty SelectedPeak_Changed_Property = DependencyProperty.Register(nameof(SelectedPeak_Changed_Action), typeof(Action<Peak?>), typeof(PlotControl), new FrameworkPropertyMetadata(null));
        public Action<Peak?>? SelectedPeak_Changed_Action
        {
            get
            {
                return (Action<Peak?>?)GetValue(SelectedPeak_Changed_Property);
            }
            set
            {
                SetValue(SelectedPeak_Changed_Property, value);
            }
        }

        /// <summary>
        /// Spectrum
        /// </summary>
        public List<SignalPoint>? SpectrumPoints { get; private set; }
        public SignalPlotXY? SpectrumPlot { get; private set; }

        /// <summary>
        /// Peaks
        /// </summary>
        public List<Peak>? Peaks { get; private set; }
        public Peak? OriginalSelectedPeak { get; private set; }

        /// <summary>
        /// Background
        /// </summary>
        public List<SignalPoint>? BackgroundPoints { get; private set; }
        public SignalPlotXY? BackgroundPlot { get; private set; }

        public PlotControl()
        {
            InitializeComponent();
            DataContext = this;

            WpfPlotControl.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
            WpfPlotControl.Configuration.DpiStretch = true;
        }

        private void ComboBox_XAxisValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeScale();
            ComboBox_XAxisValue_SelectionChanged_Action?.Invoke(sender, e);
        }

        private void ChangeScale()
        {
            PlotSpectrum(SpectrumPoints);
            Peaks?.ForEach(peak => PlotPeak(peak));
            PlotPeak(SelectedPeak);
            PlotElementMarkers();
            PlotBackground(BackgroundPoints);

            WpfPlotControl.Refresh();
        }

        public IPlottable? PlotBackground(List<SignalPoint>? backgroundPoints)
        {
            PlotRemove(BackgroundPlot);
            BackgroundPoints = backgroundPoints;
            if (backgroundPoints is null) return null;

            SignalPlotXY backgroundPlot = new()
            {
                LineWidth = 1
            };
            backgroundPlot.UpdatePlotColor(PlotColorType.Edit);

            if (XInChannels_IsSelected) backgroundPlot.Xs = backgroundPoints.Select(point => (double)point.Channel).ToArray();
            else if (XInEnergies_IsSelected) backgroundPlot.Xs = backgroundPoints.Select(point => point.Energy).ToArray();

            if (LinearScale_IsSelected) backgroundPlot.Ys = backgroundPoints.Select(point => (double)point.Value).ToArray();
            else if (LogarithmicScale_IsSelected) backgroundPlot.Ys = backgroundPoints.Select(point => point.ValueLog).ToArray();

            backgroundPlot.MaxRenderIndex = backgroundPoints.Count - 1;
            backgroundPlot.FillBelow();

            BackgroundPlot = backgroundPlot;
            PlotAdd(backgroundPlot);
            return backgroundPlot;
        }

        private void ComboBox_YAxisScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeScale();
            ComboBox_YAxisValue_SelectionChanged_Action?.Invoke(sender, e);
        }

        private void Wpfplot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SpectrumPlot is null || SpectrumPoints is null) return;

            Peak? deletedPeak = null;
            (double x, double y, int index) = WpfPlotControl.GetNearestPointIndex(SpectrumPlot);

            if (CanAddPeak && SelectedPeak == null)
            {
                SignalPoint? signalPointByClick = null;
                if (XInChannels_IsSelected) signalPointByClick = SpectrumPoints.FirstOrDefault(point => point.Channel == x);
                else if (XInEnergies_IsSelected) signalPointByClick = SpectrumPoints.FirstOrDefault(point => point.Energy == x);
                SelectedPeak = this.AddSignalPoint(signalPointByClick, x, y);
                PlotRefresh();
            }
            else if (CanEditPeak && SelectedPeak == null)
            {
                Peak? peakToEdit = null;
                if (XInChannels_IsSelected) peakToEdit = Peaks?.FirstOrDefault(peak => peak.IsInChannelInterval(x));
                else if (XInEnergies_IsSelected) peakToEdit = Peaks?.FirstOrDefault(peak => peak.IsInEnergyInterval(x));
                if (peakToEdit is not null)
                {
                    OriginalSelectedPeak = peakToEdit.JsonCopy();
                    SelectedPeak = peakToEdit;
                    SelectedPeak.SignalPlot?.UpdatePlotColor(PlotColorType.Edit);
                    PlotRefresh();
                }
            }
            else if (CanRemovePeak)
            {
                if (XInChannels_IsSelected)
                {
                    deletedPeak = Peaks?.FirstOrDefault(peak => peak.IsInChannelInterval(x));
                }
                else if (XInEnergies_IsSelected)
                {
                    deletedPeak = Peaks?.FirstOrDefault(peak => peak.IsInEnergyInterval(x));
                }

                if (deletedPeak is not null)
                {
                    Peaks?.Remove(deletedPeak);
                    PlotRemoveWithRefresh(deletedPeak.SignalPlot);
                }
            }
            else if (CanSelectPeak)
            {
                if (SelectedPeak == null)
                {
                    Peak? clickedPeak = null;

                    if (XInChannels_IsSelected) clickedPeak = Peaks?.FirstOrDefault(peak => peak.IsInChannelInterval(x));
                    else if (XInEnergies_IsSelected) clickedPeak = Peaks?.FirstOrDefault(peak => peak.IsInEnergyInterval(x));

                    SelectedPeak = clickedPeak;
                    SelectedPeak?.SignalPlot?.UpdatePlotColor(PlotColorType.Select);
                    PlotRefresh();
                }
                else
                {
                    SelectedPeak?.SignalPlot?.UpdatePlotColor();
                    PlotRefresh();
                    SelectedPeak = null;
                }
            }

            Wpfplot_MouseLeftButtonDown_Action?.Invoke(sender, e, deletedPeak);
        }

        private void Wpfplot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Wpfplot_MouseLeftButtonUp_Action?.Invoke(sender, e);
        }

        private void Wpfplot_MouseMove(object sender, MouseEventArgs e)
        {
            bool mouseIsDown = Mouse.LeftButton == MouseButtonState.Pressed;
            if (mouseIsDown == false) return;
            if (SpectrumPlot is null || SelectedPeak?.SignalPlot is null || SpectrumPoints is null) return;

            if (CanAddPeak || CanEditPeak)
            {
                int leftPeakIndex = Array.IndexOf(SpectrumPlot.Xs, SelectedPeak.SignalPlot.Xs.First());
                int rightPeakIndex = Array.IndexOf(SpectrumPlot.Xs, SelectedPeak.SignalPlot.Xs.Last());

                double mouseX = WpfPlotControl.GetMouseCoordinates().x;
                int closestIndex = WpfPlotControl.GetNearestPointIndex(SpectrumPlot).index;
                double? closestValue = null;
                if (XInChannels_IsSelected) closestValue = SpectrumPoints.Select(point => (double)point.Channel).ToList().ClosestValue(mouseX);
                else if (XInEnergies_IsSelected) closestValue = SpectrumPoints.Select(point => point.Energy).ToList().ClosestValue(mouseX);
                closestIndex = Array.IndexOf(SpectrumPlot.Xs, closestValue);
                List<int> tempArray = new()
                {
                    closestIndex,
                    (Math.Abs(closestIndex - leftPeakIndex) > Math.Abs(closestIndex - rightPeakIndex)) ? leftPeakIndex : rightPeakIndex
                };

                int leftPlotIndex = tempArray.Min();
                int rightPlotIndex = tempArray.Max();
                if (leftPlotIndex == rightPlotIndex) return;

                SelectedPeak.SignalPlot.Xs = SpectrumPlot.Xs.Skip(leftPlotIndex).Take(rightPlotIndex - leftPlotIndex + 1).ToArray();
                SelectedPeak.SignalPlot.Ys = SpectrumPlot.Ys.Skip(leftPlotIndex).Take(rightPlotIndex - leftPlotIndex + 1).ToArray();
                SelectedPeak.SignalPlot.MaxRenderIndex = SelectedPeak.SignalPlot.Xs.Length - 1;

                double leftPlotXValue = SelectedPeak.SignalPlot.Xs.Min();
                double rightPlotXValue = SelectedPeak.SignalPlot.Xs.Max();

                if (XInChannels_IsSelected)
                {
                    SelectedPeak.LeftBaseSignalPoint = SpectrumPoints.First(point => point.Channel == leftPlotXValue);
                    SelectedPeak.RightBaseSignalPoint = SpectrumPoints.First(point => point.Channel == rightPlotXValue);
                }
                else if (XInEnergies_IsSelected)
                {
                    SelectedPeak.LeftBaseSignalPoint = SpectrumPoints.First(point => point.Energy == leftPlotXValue);
                    SelectedPeak.RightBaseSignalPoint = SpectrumPoints.First(point => point.Energy == rightPlotXValue);
                }

                double maxYValue = SelectedPeak.SignalPlot.Ys.Max();
                SignalPoint? highestPoint = null;
                if (LinearScale_IsSelected) highestPoint = SpectrumPoints.FirstOrDefault(point => point.Value == maxYValue);
                else if (LogarithmicScale_IsSelected) highestPoint = SpectrumPoints.FirstOrDefault(point => point.ValueLog == maxYValue);
                if (highestPoint != null) SelectedPeak.HighestSignalPoint = highestPoint;
                SelectedPeak.OnPropertyChanged();
                WpfPlotControl.Refresh();
            }

            Wpfplot_MouseMove_Action?.Invoke(sender, e);
        }

        public void ResetSelectedPeak(Peak? previousPeakState = null)
        {
            if (SelectedPeak?.SignalPlot is not null && previousPeakState is not null && Peaks is not null)
            {
                int originalPeakIndex = Peaks.FindIndex(peak => peak.Id == previousPeakState.Id);
                if (originalPeakIndex >= 0)
                {
                    PlotRemove(SelectedPeak.SignalPlot);
                    Peaks[originalPeakIndex] = previousPeakState;
                    PlotPeak(previousPeakState);
                    PlotRefresh();
                }
            }

            SelectedPeak?.SignalPlot?.UpdatePlotColor();
            PlotRefresh();
            SelectedPeak = null;
        }

        public IPlottable? PlotSpectrum(List<SignalPoint>? spectrumPoints)
        {
            PlotRemove(SpectrumPlot);
            SpectrumPoints = spectrumPoints;
            if (spectrumPoints is null) return null;

            SignalPlotXY spectrumPlot = new()
            {
                Color = Color.Red,
                LineWidth = 1
            };

            if (XInChannels_IsSelected) spectrumPlot.Xs = spectrumPoints.Select(point => (double)point.Channel).ToArray();
            else if (XInEnergies_IsSelected) spectrumPlot.Xs = spectrumPoints.Select(point => point.Energy).ToArray();

            if (LinearScale_IsSelected) spectrumPlot.Ys = spectrumPoints.Select(point => (double)point.Value).ToArray();
            else if (LogarithmicScale_IsSelected) spectrumPlot.Ys = spectrumPoints.Select(point => point.ValueLog).ToArray();

            spectrumPlot.MaxRenderIndex = spectrumPoints.Count - 1;
            spectrumPlot.FillBelow();

            //WpfplotSpectrum.Plot.Title("One Million Points");
            WpfPlotControl.Plot.XAxis.LockLimits(false);
            WpfPlotControl.Plot.YAxis.LockLimits(false);
            WpfPlotControl.Plot.SetAxisLimitsX(0, spectrumPlot.Xs.Max() * 1.05);
            WpfPlotControl.Plot.SetAxisLimitsY(0, spectrumPlot.Ys.Max() * 1.05);
            WpfPlotControl.Plot.XAxis.LockLimits(true);
            WpfPlotControl.Plot.YAxis.LockLimits(true);
            //WpfplotSpectrum.Plot.SaveFig("quickstart_signal.png");

            SpectrumPlot = spectrumPlot;
            PlotAdd(spectrumPlot);
            return spectrumPlot;
        }

        private IPlottable? PlotPeak(Peak? peak)
        {
            if (peak is null) return null;
            if (SpectrumPoints is null || peak.LeftBaseSignalPoint is null || peak.RightBaseSignalPoint is null) return null;

            List<SignalPoint> peakPoints = SpectrumPoints.Skip(peak.LeftBaseSignalPoint.Channel - 1).Take(peak.RightBaseSignalPoint.Channel - peak.LeftBaseSignalPoint.Channel + 1).ToList();
            SignalPlotXY peakPlot = new()
            {
                Color = peak.SignalPlot?.Color ?? Color.Blue,
                LineWidth = 1
            };
            PlotRemove(peak.SignalPlot);

            if (XInChannels_IsSelected) peakPlot.Xs = peakPoints.Select(point => (double)point.Channel).ToArray();
            else if (XInEnergies_IsSelected) peakPlot.Xs = peakPoints.Select(point => point.Energy).ToArray();

            if (LinearScale_IsSelected) peakPlot.Ys = peakPoints.Select(point => (double)point.Value).ToArray();
            else if (LogarithmicScale_IsSelected) peakPlot.Ys = peakPoints.Select(point => point.ValueLog).ToArray();

            peakPlot.MaxRenderIndex = peakPoints.Count - 1;
            peakPlot.FillBelow();

            peak.SignalPlot = peakPlot;
            PlotAdd(peakPlot);
            return peakPlot;
        }

        public void PlotPeaks(List<Peak>? peaks)
        {
            if (Peaks is not null)
            {
                foreach (Peak peak in Peaks)
                {
                    PlotRemove(peak.SignalPlot);
                }
            }
            Peaks = new();

            if (peaks is null) return;

            SelectedPeak = null;
            OriginalSelectedPeak = null;

            foreach (Peak peak in peaks)
            {
                PlotPeak(peak);
                Peaks.Add(peak);
            }

            WpfPlotControl.Refresh();
        }

        public void PlotElementMarkers()
        {
            if (Peaks is null) return;

            foreach (Peak peak in Peaks)
            {
                PlotRemove(peak.ElementMarker?.MarkerPlot);
                PlotRemove(peak.ElementMarker?.TextPlot);
            }

            foreach (Peak peak in Peaks)
            {
                PlotElementMarker(peak);
            }

            WpfPlotControl.Refresh();
        }

        private ElementMarker? PlotElementMarker(Peak? peak)
        {
            if (peak?.ElementLine is null) return null;
            peak.ElementMarker = new();
            peak.ElementMarker.MarkerPlot = new()
            {
                MarkerShape = peak.ElementMarker.MarkerShape,
                MarkerSize = peak.ElementMarker.MarkerSize,
                Color = peak.ElementMarker.MarkerColor
            };
            PlotRemove(peak.ElementMarker.MarkerPlot);

            if (XInChannels_IsSelected) peak.ElementMarker.MarkerPlot.X = peak.HighestSignalPoint.Channel;
            else if (XInEnergies_IsSelected) peak.ElementMarker.MarkerPlot.X = peak.HighestSignalPoint.Energy;

            if (LinearScale_IsSelected) peak.ElementMarker.MarkerPlot.Y = peak.HighestSignalPoint.Value;
            else if (LogarithmicScale_IsSelected) peak.ElementMarker.MarkerPlot.Y = peak.HighestSignalPoint.ValueLog;

            peak.ElementMarker.TextPlot = new()
            {
                Label = (peak.ElementLine?.Info?.Symbol ?? string.Empty) + (" " + peak.ElementLine?.Line ?? string.Empty),
                FontSize = peak.ElementMarker.TextSize,
                Color = peak.ElementMarker.TextColor
            };
            PlotRemove(peak.ElementMarker.TextPlot);

            if (XInChannels_IsSelected) peak.ElementMarker.TextPlot.X = peak.HighestSignalPoint.Channel;
            else if (XInEnergies_IsSelected) peak.ElementMarker.TextPlot.X = peak.HighestSignalPoint.Energy;

            if (LinearScale_IsSelected) peak.ElementMarker.TextPlot.Y = peak.HighestSignalPoint.Value;
            else if (LogarithmicScale_IsSelected) peak.ElementMarker.TextPlot.Y = peak.HighestSignalPoint.ValueLog;

            PlotAdd(peak.ElementMarker.MarkerPlot);
            PlotAdd(peak.ElementMarker.TextPlot);
            return peak.ElementMarker;
        }

        public void PlotClear()
        {
            WpfPlotControl.Plot.Clear();
            WpfPlotControl.Refresh();

            LinearScale_IsSelected = true;
            XInChannels_IsSelected = true;

            SpectrumPoints = null;
            SpectrumPlot = null;

            Peaks = null;
            SelectedPeak = null;
            OriginalSelectedPeak = null;

            BackgroundPoints = null;
            BackgroundPlot = null;
        }

        public void PlotRefresh()
        {
            WpfPlotControl.Refresh();
        }

        public void PlotAdd(IPlottable plot)
        {
            WpfPlotControl.Plot.Add(plot);
        }

        public void PlotAddWithRefresh(IPlottable plot)
        {
            PlotAdd(plot);
            PlotRefresh();
        }

        public void PlotRemove(IPlottable? plot)
        {
            WpfPlotControl.Plot.Remove(plot);
        }

        public void PlotRemoveWithRefresh(IPlottable? plot)
        {
            PlotRemove(plot);
            PlotRefresh();
        }
    }
}
