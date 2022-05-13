﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XRayAnalyzer.Objects.Converters
{
    internal class TwoConverters : IValueConverter
    {
        public IValueConverter? Converter1 { get; set; }
        public IValueConverter? Converter2 { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object? convertedValue = Converter1?.Convert(value, targetType, parameter, culture);
            return Converter2?.Convert(convertedValue, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
