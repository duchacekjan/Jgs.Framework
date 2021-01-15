using Jgs.UI.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Jgs.UI.Converters
{
    public class ContrastColorConverter : AValueConverter
    {
        public Color Dark { get; set; } = Colors.Black;

        public Color Light { get; set; } = Colors.White;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            switch (value)
            {
                case Brush brush:
                    result = brush.ContrastColor(Dark, Light);
                    break;
                case Color color:
                    result = color.ContrastColor(Dark, Light);
                    break;
            }

            if (result is Color resultColor)
            {
                if (targetType.IsAssignableFrom(typeof(Brush)))
                {
                    result = new SolidColorBrush(resultColor);
                }
                else
                {
                    result = resultColor;
                }
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
