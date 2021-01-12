﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jgs.Framework.UI.Converters
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class AValueConverter : MarkupExtension, IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
