using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Converters
{
	internal class NullConverter : IValueConverter, IMarkupExtension
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (parameter is not string param)
			{
				throw new InvalidDataException();
			}

			var parameters = param.Split(';');
			var finalValue = value + "";
			if (parameters.Length > 1)
			{
				var concat = parameters[1];
				var start = concat.IndexOf("|");
				var substring = "";
				if (start == 0)
				{
					substring = concat.Substring(1, concat.Length - 1);
					finalValue += substring;
				}
				else
				{
					substring = concat.Substring(0, start);
					finalValue = finalValue.Insert(0, substring);
				}
			}

			if (value == null)
				return parameters[0];
			else
				return finalValue;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
