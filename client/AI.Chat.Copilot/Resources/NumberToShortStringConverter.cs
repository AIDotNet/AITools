using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Resources
{
    public class NumberToShortStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IConvertible number)
            {
                double doubleValue = number.ToDouble(CultureInfo.InvariantCulture);
                if (doubleValue >= 1000000)
                    return (doubleValue / 1000000).ToString("0.#") + "m";
                else if (doubleValue >= 1000)
                    return (doubleValue / 1000).ToString("0.#") + "k";
                else
                    return doubleValue.ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
