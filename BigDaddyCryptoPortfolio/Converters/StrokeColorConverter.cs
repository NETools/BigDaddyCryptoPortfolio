using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Converters
{
    public class StrokeColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var result = (bool)value;
            var parameters = ((string)parameter).Trim().Split(",");

            var colors = parameters.Select(p => Color.FromArgb(p)).ToArray();

            return result ? colors[1] : colors[0];
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
