using Jgs.Framework.UI.Markup;
using System;
using System.Globalization;
using System.Windows;

namespace Jgs.Framework.UI.Converters
{
    public class EnumValuesConverter : AValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value is Type enumType && enumType.IsEnum)
            {
                var markup = new EnumValues
                {
                    EnumType = enumType
                };
                result = markup.ProvideValue(null);
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
