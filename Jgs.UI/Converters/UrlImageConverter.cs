using Jgs.UI.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Jgs.UI.Converters
{
    public class UrlImageConverter : AValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value is string urlImage && targetType==typeof(ImageSource))
            {
                result = urlImage.UrlAsImageSource();
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
