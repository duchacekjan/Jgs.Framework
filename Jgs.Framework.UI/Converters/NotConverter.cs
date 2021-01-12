using System;
using System.Globalization;

namespace Jgs.Framework.UI.Converters
{
    public class NotConverter : AValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (value as bool?) ?? false;
            return !result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (value as bool?) ?? false;
            return !result;
        }
    }
}
