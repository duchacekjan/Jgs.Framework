using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Jgs.UI.Components
{
    public class PathButton : Button
    {
        public static readonly DependencyProperty PathProperty;

        static PathButton()
        {
            var owner = typeof(PathButton);
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
