using Jgs.Framework.UI.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace Jgs.Framework.UI.Components.LoginViewCore
{
    public class HasFlagConverter : AValueConverter
    {
        public AllowedEmpty Flag { get; set; } = AllowedEmpty.None;

        public bool Inverse { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value is AllowedEmpty partVisibility)
            {
                var hasFlag = partVisibility.HasFlag(Flag);
                if (Inverse)
                {
                    hasFlag = !hasFlag;
                }

                result = hasFlag;
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
