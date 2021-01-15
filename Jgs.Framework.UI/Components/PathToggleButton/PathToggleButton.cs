using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace Jgs.Framework.UI.Components
{
    public class PathToggleButton : ToggleButton
    {
        public static readonly DependencyProperty PathProperty;

        static PathToggleButton()
        {
            var owner = typeof(PathToggleButton);
            PathProperty = DependencyProperty.Register(nameof(Path), typeof(Path), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public Path Path
        {
            get => (Path)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
    }
}
