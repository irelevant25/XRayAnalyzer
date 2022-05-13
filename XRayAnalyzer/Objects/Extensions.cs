using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XRayAnalyzer.Objects
{
    public static class Extensions
    {
        public static T RemoveAtAndGet<T>(this IList<T> list, int index)
        {
            lock (list)
            {
                T value = list[index];
                list.RemoveAt(index);
                return value;
            }
        }

        public static long ToMiliseconds(this DateTime datetime)
        {
            return datetime.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static string JsonSerialize<T>(this T obj, bool writeIndented = false)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = writeIndented
            };
            return JsonSerializer.Serialize(obj, options);
        }

        public static T? JsonDeserialize<T>(this string obj)
        {
            return JsonSerializer.Deserialize<T>(obj);
        }

        public static T? JsonCopy<T>(this T obj)
        {
            string json = JsonSerializer.Serialize(obj);
            return string.IsNullOrEmpty(json) ? default : json.JsonDeserialize<T>();
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T>? addCollection)
        {
            if (addCollection == null)
            {
                return;
            }
            foreach (var i in addCollection)
            {
                collection.Add(i);
            }
        }

        public static double ClosestValue(this List<double> values, double value)
        {
            double closestValue = double.PositiveInfinity;
            double lowestDiff = double.PositiveInfinity;
            foreach (double item in values)
            {
                double currentDiff = Math.Abs(item - value);
                if (lowestDiff >= currentDiff)
                {
                    lowestDiff = currentDiff;
                    closestValue = item;
                }
                else
                {
                    break;
                }
            }
            return closestValue;
        }

        public static void AddRangeExtended<T>(this List<T> collection, IEnumerable<T>? addCollection)
        {
            if (addCollection == null)
            {
                return;
            }
            foreach (var i in addCollection)
            {
                collection.Add(i);
            }
        }

        public static void ComboBoxViewsAddItem(this MenuItem menuItem, string propertyName, object view, bool isSelected = false)
        {
            Binding binding = new ()
            {
                Path = new PropertyPath("(0)", typeof(StringsResource).GetProperty(propertyName))
            };
            ComboBoxItem comboBoxItem = new()
            {
                Tag = view,
                IsSelected = isSelected
            };
            comboBoxItem.SetBinding(ContentControl.ContentProperty, binding);

            menuItem.Items.Add(comboBoxItem);
        }

        public static MenuItem MenuItemAdd(this MenuItem menuItem, string propertyName, RoutedEventHandler? clickCallback = null, object? tag = null)
        {
            MenuItem newMenuItem = new()
            {
                IsCheckable = true,
                Tag = tag
            };
            Binding bindingHeader = new ()
            {
                Path = new PropertyPath("(0)", typeof(StringsResource).GetProperty(propertyName))
            };
            newMenuItem.SetBinding(HeaderedItemsControl.HeaderProperty, bindingHeader);
            if (clickCallback is not null) newMenuItem.Click += clickCallback;
            if (propertyName == Properties.Settings.Default.Language || propertyName == Properties.Settings.Default.View) newMenuItem.IsChecked = true;
            menuItem.Items.Add(newMenuItem);
            return newMenuItem;
        }
    }
}
