﻿using System;
using System.Globalization;
using System.Windows;

namespace Jgs.UI.Converters
{
    public class EnumConverter : AValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value != null && targetType != null && targetType.IsEnum)
            {
                var numValue = System.Convert.ToInt64(value);
                result = Enum.ToObject(targetType, numValue);
            }
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
