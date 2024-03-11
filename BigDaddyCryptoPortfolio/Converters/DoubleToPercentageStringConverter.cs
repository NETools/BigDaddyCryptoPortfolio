using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Converters
{
    public class DoubleToPercentageStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not double percentage)
                return "";

            if (percentage <= double.Epsilon)
                return "";

            var formattedPercentage = Math.Round(percentage * 100.0, 1).ToString().Replace(".", ",");

            return $"{formattedPercentage}%";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
