﻿using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Jgs.Framework.UI.Converters
{
    public class CollectionEmptyConverter : AValueWithNegateConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = true;
            if (value is IEnumerable enumerable)
            {
                result = enumerable.Cast<object>().Count() == 0;
            }

            if (Negate)
            {
                result = !result;
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
