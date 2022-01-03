using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Jgs.UI.GridLayout
{
    public static partial class GridLayout
    {
        private const string Limits = nameof(Limits);
        private const string Row = nameof(Row);
        private const string Column = nameof(Column);
        private const string RowSpan = nameof(RowSpan);
        private const string ColumnSpan = nameof(ColumnSpan);
        private const string IsEnabled = nameof(IsEnabled);
        private const string IsChildEnabled = nameof(IsChildEnabled);

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            IsEnabled,
            typeof(bool),
            typeof(GridLayout),
            new FrameworkPropertyMetadata(false, IsEnabledChanged));
        public static readonly DependencyProperty IsChildEnabledProperty = DependencyProperty.RegisterAttached(
            IsChildEnabled,
            typeof(bool),
            typeof(GridLayout),
            new FrameworkPropertyMetadata(true, IsChildEnabledChanged));

        public static readonly DependencyProperty LimitsProperty = Register(Limits, LayoutChanged);
        public static readonly DependencyProperty ColumnProperty = Register(Column, LayoutChanged);
        public static readonly DependencyProperty ColumnSpanProperty = Register(ColumnSpan, LayoutChanged);
        public static readonly DependencyProperty RowProperty = Register(Row, LayoutChanged);
        public static readonly DependencyProperty RowSpanProperty = Register(RowSpan, LayoutChanged);

#if NET6_0
        public static readonly LayoutLimitCollection DefaultLimits = new(".c-700.s-1920");
#else
        public static readonly LayoutLimitCollection DefaultLimits = new LayoutLimitCollection(".c-700.s-1920");
#endif
        public static readonly IDictionary<DependencyProperty, DependencyProperty> ToGridPropertiesMap = new Dictionary<DependencyProperty, DependencyProperty>
        {
            { ColumnProperty, Grid.ColumnProperty },
            { RowProperty, Grid.RowProperty },
            { ColumnSpanProperty, Grid.ColumnSpanProperty },
            { RowSpanProperty, Grid.RowSpanProperty }
        };

        private static DependencyProperty Register(string propertyName, PropertyChangedCallback callback)
        {
            return DependencyProperty.RegisterAttached(propertyName, typeof(string), typeof(GridLayout), Meta(callback));
        }

        private static FrameworkPropertyMetadata Meta(PropertyChangedCallback callback)
        {
            return new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, callback);
        }

        public static bool GetIsEnabled(Grid element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(Grid element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsChildEnabled(FrameworkElement element)
        {
            return (bool)element.GetValue(IsChildEnabledProperty);
        }

        public static void SetIsChildEnabled(FrameworkElement element, bool value)
        {
            element.SetValue(IsChildEnabledProperty, value);
        }

        public static void SetLimits(Grid element, string value)
        {
            element.SetValue(LimitsProperty, value);
        }

        public static string GetLimits(Grid element)
        {
            return (string)element.GetValue(LimitsProperty);
        }
        public static void SetColumn(FrameworkElement element, string value)
        {
            element.SetValue(ColumnSpanProperty, value);
        }

        public static string GetColumn(FrameworkElement element)
        {
            return (string)element.GetValue(ColumnProperty);
        }

        public static void SetColumnSpan(FrameworkElement element, string value)
        {
            element.SetValue(ColumnSpanProperty, value);
        }

        public static string GetColumnSpan(FrameworkElement element)
        {
            return (string)element.GetValue(ColumnSpanProperty);
        }

        public static void SetRow(FrameworkElement element, string value)
        {
            element.SetValue(RowProperty, value);
        }

        public static string GetRow(FrameworkElement element)
        {
            return (string)element.GetValue(RowProperty);
        }

        public static void SetRowSpan(FrameworkElement element, string value)
        {
            element.SetValue(RowSpanProperty, value);
        }

        public static string GetRowSpan(FrameworkElement element)
        {
            return (string)element.GetValue(RowSpanProperty);
        }
    }
}
