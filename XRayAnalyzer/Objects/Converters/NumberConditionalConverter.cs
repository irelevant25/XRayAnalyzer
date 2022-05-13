using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XRayAnalyzer.Objects.Converters
{
    internal class NumberConditionalConverter : IValueConverter
    {
        public int? From { get; set; }
        public int? To { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int number;

            if (value is int)
            {
                number = (int)value;
            }
            else if (value is ICollection)
            {
                number = ((ICollection)value).Count;
            }
            else
            {
                throw new NotSupportedException("NumberConditionalConverter value have to be type int or ICollection.");
            }

            if (From != null && To != null)
            {
                return From <= number && number < To;
            }
            if (From != null)
            {
                return From <= number;
            }
            else if (To != null)
            {
                return number < To;
            }
            else
            {
                throw new NotSupportedException("NumberConditionalConverter at least one of property, From or To, have to has value.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("NumberConditionalConverter is a OneWay converter.");
        }
    }
}