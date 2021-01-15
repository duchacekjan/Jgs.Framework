using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Jgs.Framework.UI.Converters
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
                    result = ContrastColor(brush);
                    break;
                case Color color:
                    result = ContrastColor(color);
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

        private Color ContrastColor(Brush brush)
        {
            var original = (SolidColorBrush)brush;
            return ContrastColor(original.Color);
        }

        private Color ContrastColor(Color iColor)
        {
            var luma = ((0.299 * iColor.R) + (0.587 * iColor.G) + (0.114 * iColor.B)) / 255;
            return luma > 0.5 ? Dark : Light;
        }
    }
}
