using Jgs.UI.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace Jgs.UI.Components.VolumeControlCore
{
    public class VolumeLevelConverter : AValueConverter
    {
        public static double MuteLevel => 0;

        public static double Volume0Level => 0.3;

        public static double Volume1Level => 0.65;

        public static double Volume2Level => 1;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value is double volume)
            {
                if (volume <= MuteLevel)
                {
                    result = -1;
                }
                else if (volume <= Volume0Level)
                {
                    result = 0;
                }
                else if (volume <= Volume1Level)
                {
                    result = 1;
                }
                else if (volume <= Volume2Level)
                {
                    result = 2;
                }
                else
                {
                    result = 3;
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
