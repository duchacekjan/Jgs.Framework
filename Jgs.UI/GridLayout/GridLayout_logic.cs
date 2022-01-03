using System;
using System.Windows;
using System.Windows.Controls;

namespace Jgs.UI.GridLayout
{
    public static partial class GridLayout
    {
        private static void LayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                SetLayout(grid);
            }
        }

        private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                if (e.OldValue is bool oldAttached && oldAttached)
                {
                    grid.SizeChanged -= OnGridSizeChanged;
                }

                if (e.NewValue is bool newAttached && newAttached)
                {
                    grid.SizeChanged += OnGridSizeChanged;
                }

                SetLayout(grid);
            }
        }

        private static void IsChildEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (element.Parent is Grid elementsGrid)
                {
                    SetLayout(elementsGrid, element);
                }
            }
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
            if (GetIsEnabled(grid))
            {
                var limits = getCurrentLimits(grid);

                if (LayoutStateChanged(grid.ActualWidth, oldWidth, limits, out var state))
                {
                    UpdateLayout(grid, state, SetElementLayout);
                }
            }
        }

        private static void SetLayout(Grid grid, FrameworkElement element)
        {
            if (GetIsEnabled(grid) && GetIsChildEnabled(element))
            {
                var limits = getCurrentLimits(grid);
                var state = limits.GetLayoutType(grid.ActualWidth);
                if (state.HasValue)
                {
                    SetElementLayout(element, state.Value);
                }
            }
        }

        private static LayoutLimitCollection getCurrentLimits(Grid grid)
        {
            var limitsArguments = GetLimits(grid);
            return string.IsNullOrEmpty(limitsArguments)
                ? DefaultLimits
                : new LayoutLimitCollection(limitsArguments);

        }

        private static void SetElementLayout(FrameworkElement element, LayoutArgumentType type)
        {
            element.SetGridLayoutValue(ColumnProperty, type);
            element.SetGridLayoutValue(RowProperty, type);
            element.SetGridLayoutValue(ColumnSpanProperty, type);
            element.SetGridLayoutValue(RowSpanProperty, type);
        }

        private static void UpdateLayout(Grid grid, LayoutArgumentType type, Action<FrameworkElement, LayoutArgumentType> action)
        {
            foreach (var child in grid.Children)
            {
                if (child is FrameworkElement element && GetIsChildEnabled(element))
                {
                    action?.Invoke(element, type);
                }
            }
        }

        private static bool LayoutStateChanged(double gridWidth, double? oldWidth, LayoutLimitCollection limits, out LayoutArgumentType newLayoutState)
        {
            var newState = limits.GetLayoutType(gridWidth);
            var result = true;
            newLayoutState = newState ?? throw new ArgumentOutOfRangeException(nameof(gridWidth));
            var oldState = limits.GetLayoutType(oldWidth);
            if (oldState.HasValue)
            {
                result = newState != oldState;
            }
            return result;
        }
    }
}
