using System.Windows;

namespace Jgs.UI.GridLayoutManager
{
    public static partial class LayoutManager
    {
        private const string Limits = nameof(Limits);
        private const string Row = nameof(Row);
        private const string Column = nameof(Column);
        private const string RowSpan = nameof(RowSpan);
        private const string ColumnSpan = nameof(ColumnSpan);
        private const string IsEnabled = nameof(IsEnabled);

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            IsEnabled, 
            typeof(bool), 
            typeof(LayoutManager), 
            new FrameworkPropertyMetadata(false, IsEnabledChanged));

        public static readonly DependencyProperty CompactLimitProperty = Register(Limits, LimitsChanged);

#if NET6_0
        public static readonly LayoutArgumentCollection DefaultLimits = new(".c-700.s-1920");
#else
        public static readonly LayoutArgumentCollection DefaultLimits = new LayoutArgumentCollection(".c-700.s-1920");
#endif

        private static DependencyProperty Register(string propertyName, PropertyChangedCallback callback)
        {
            return DependencyProperty.RegisterAttached(propertyName, typeof(string), typeof(LayoutManager), Meta(callback));
        }

        private static FrameworkPropertyMetadata Meta(PropertyChangedCallback callback)
        {
            return new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, callback);
        }
    }
}
