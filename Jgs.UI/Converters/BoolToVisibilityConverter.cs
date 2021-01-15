﻿using System;
using System.Globalization;
using System.Windows;

namespace Jgs.UI.Converters
{
    public class BoolToVisibilityConverter : AValueWithNegateConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = Visibility.Collapsed;
            if (value is bool isVisible)
            {
                if ((!Negate && isVisible) ||
                    (Negate && !isVisible))
                {
                    result = Visibility.Visible;
                }
            }
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;
            if (value is Visibility visibility)
            {
                result = visibility == Visibility.Visible;
                if (Negate)
                {
                    result = !result;
                }
            }
            return result;
        }
    }
}
