using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayAnalyzer.Controls;
using XRayAnalyzer.MVVM.Model;
using XRayAnalyzer.Objects.Enums;

namespace XRayAnalyzer.Objects
{
    public static class Helper
    {
        private static MainModel MainModel { get; set; } = MainModel.Instance;

        public static T[] ArrayMerge<T>(T[] source, T[] destination, bool reverse = false)
        {
            T[] newArray = new T[source.Length + destination.Length];
            if (reverse)
            {
                source.CopyTo(newArray, 0);
                destination.CopyTo(newArray, source.Length);
            }
            else
            {
                destination.CopyTo(newArray, 0);
                source.CopyTo(newArray, destination.Length);
            }
            return newArray;
        }

        public static T[] ArrayReduce<T>(T[] source, int reduceAbout, bool reverse = false)
        {
            T[] newArray = new T[source.Length - reduceAbout];
            if (reverse)
            {
                Array.Copy(source, reduceAbout, newArray, 0, newArray.Length);
            }
            else
            {
                Array.Copy(source, 0, newArray, 0, newArray.Length);
            }
            return newArray;
        }

        public static List<int> ArrayRange(int from, int to)
        {
            List<int> range = new (to - from);
            for (; from < to; from++)
            {
                range.Add(from);
            }
            return range;
        }

        public static List<int> ArrayRange(int to)
        {
            int from = 0;
            List<int> range = new (to - from);
            for (; from < to; from++)
            {
                range.Add(from);
            }
            return range;
        }

        public static bool CloseToFirst(this double[] x, double xValue)
        {
            double diffLeft = Math.Abs(x.First() - xValue);
            double diffRight = Math.Abs(x.Last() - xValue);
            return diffLeft < diffRight;
        }

        public static double ChannelToEnergy(int channel)
        {
            MainModel model = MainModel.Instance;
            if (model.Data.Calibration is null) return default;
            return Math.Round(model.Data.Calibration.Intercept + model.Data.Calibration.Slope * channel, 2);
        }

        public static double ValueToValueLog(double value)
        {
            MainModel model = MainModel.Instance;
            if (model.Data.LogBase is null) return default;
            return double.IsInfinity(Math.Log(value, (double)model.Data.LogBase)) ? 0 : Math.Log(value, (double)model.Data.LogBase);
        }

        public static (double x, double y, int index) GetNearestPointIndex(this WpfPlot wpfPlot, IHasPointsGenericX<double, double> genericPlot, double? x = null)
        {
            if (genericPlot == null)
            {
                return (0, 0, 0);
            }

            if (x == null)
            {
                x = wpfPlot.GetMouseCoordinates().x;
            }

            return genericPlot.GetPointNearestX((double)x);
        }

        public static void UpdatePlotColor(this SignalPlotXY plot, PlotColorType? colorType = null)
        {
            plot.Color = colorType switch
            {
                PlotColorType.Add => Color.DarkOrange,
                PlotColorType.Default => Color.Blue,
                PlotColorType.Edit => Color.DarkCyan,
                PlotColorType.Select => Color.DarkGreen,
                PlotColorType.Background => Color.Black,
                _ => Color.Blue,
            };
            plot.FillBelow();
            //plotControl.PlotRefresh();
        }

        public static bool IsInEnergyInterval(this Peak peak, double value)
        {
            if (peak.LeftBaseSignalPoint is null || peak.RightBaseSignalPoint is null) return false;
            return peak.LeftBaseSignalPoint.Energy <= value && peak.RightBaseSignalPoint.Energy >= value;
        }

        public static bool IsInChannelInterval(this Peak peak, double value)
        {
            if (peak.LeftBaseSignalPoint is null || peak.RightBaseSignalPoint is null) return false;
            return peak.LeftBaseSignalPoint.Channel <= value && peak.RightBaseSignalPoint.Channel >= value;
        }

        public static Peak? AddSignalPoint(this PlotControl plotControl, SignalPoint? signalPointByClick, double x, double y)
        {
            if (signalPointByClick == null) return null;

            SignalPlotXY signalPlotXY = new()
            {
                Xs = new double[] { x },
                Ys = new double[] { y },
                LineWidth = 1
            };
            signalPlotXY.UpdatePlotColor(PlotColorType.Add);

            Peak newPeak = new()
            {
                Id = DateTime.Now.ToMiliseconds(),
                HighestSignalPoint = signalPointByClick,
                SignalPlot = signalPlotXY
            };

            plotControl.PlotAddWithRefresh(signalPlotXY);
            return newPeak;
        }

        public static void IncludeElementInfo(this ElementLine elementLine)
        {
            elementLine.Info = Dataset.ElementsInfo?.ToList().Find(elementInfo => elementInfo.Number == elementLine.Element);
        }

        public static ElementInfo? GetElementInfoByNumber(int elementNumber)
        {
            return Dataset.ElementsInfo?.ToList().Find(elementInfo => elementInfo.Number == elementNumber);
        }

        public static bool CanChangeX()
        {
            return MainModel.Data.Calibration is not null;
        }

        public static bool CanChangeY()
        {
            return MainModel.Data.ReferencePoints?.Count > 0;
        }
    }
}
