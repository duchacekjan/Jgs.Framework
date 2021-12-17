using System.Windows;
using System.Windows.Controls;

namespace Jgs.UI.GridLayoutManager
{
    public static partial class LayoutManager
    {
        private static void LimitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                SetLayout(grid);
            }
        }

        private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            switch (d)
            {
                case Grid grid:
                    IsEnabledGridChanged(grid, e.OldValue, e.NewValue);
                    break;
                case FrameworkElement element:
                    IsEnabledElementChanged(element);
                    break;
            }
        }

        private static void IsEnabledElementChanged(FrameworkElement element)
        {
            if (element.Parent is Grid elementsGrid)
            {
                SetLayout(elementsGrid, element);
            }
        }

        private static void IsEnabledGridChanged(Grid grid, object oldValue, object newValue)
        {
            if (oldValue is bool oldAttached && oldAttached)
            {
                grid.SizeChanged -= OnGridSizeChanged;
            }

            if (newValue is bool newAttached && newAttached)
            {
                grid.SizeChanged += OnGridSizeChanged;
            }

            SetLayout(grid);
        }
        private static void OnGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Grid grid && e.WidthChanged)
            {
                SetLayout(grid, e.PreviousSize.Width);
            }
        }

        private static void SetLayout(Grid grid, double? oldWidth = null)
        {
            throw new System.NotImplementedException();
            //if (GetIsEnabled(grid))
            //{
            //    var limit = GetCompactLimit(grid) ?? DefaultCompactLimit;

            //    if (LayoutStateChanged(grid.ActualWidth, oldWidth, limit, out var state))
            //    {
            //        switch (state)
            //        {
            //            case LayoutState.Normal:
            //                UpdateLayout(grid, SetNormalLayout);
            //                break;
            //            case LayoutState.Compact:
            //                UpdateLayout(grid, SetCompactLayout);
            //                break;
            //        }
            //    }
            //}
        }
        private static void SetLayout(Grid grid, FrameworkElement element)
        {
            //if (GetIsEnabled(grid) && GetIsEnabled(element))
            //{
            //    var limit = GetCompactLimit(grid) ?? DefaultCompactLimit;
            //    if (grid.ActualWidth < limit)
            //    {
            //        SetCompactLayout(element);
            //    }
            //    else
            //    {
            //        SetNormalLayout(element);
            //    }
            //}
        }
    }
}
