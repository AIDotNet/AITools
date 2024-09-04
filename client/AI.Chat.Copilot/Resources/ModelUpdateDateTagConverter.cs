using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Resources
{
    public class ModelUpdateDateTagConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime time)
            {
                var diff = DateTime.Now.Subtract(time);
                if ((int)diff.TotalDays < 0)
                {
                    return $"Updated about {(int)diff.TotalHours} hours ago";
                }
                else if ((int)diff.TotalDays > 0 && (int)diff.TotalDays <= 30)
                {
                    return $"Updated {(int)diff.TotalDays} days ago";
                }
                else
                {
                    return $"Updated {time:yyyy.MM.dd}";
                }
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
