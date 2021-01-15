using Jgs.UI.Components.StripCore;
using Jgs.UI.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Jgs.UI.Components
{
    public class Strip : Control
    {
        public const string PartImage = "PART_Image";
        public static readonly DependencyProperty LinkProperty;
        public static readonly DependencyProperty CountProperty;
        public static readonly DependencyProperty IntervalProperty;
        public static readonly DependencyProperty CycleProperty;

        private readonly DispatcherTimer m_timer;

        private ImageBrush m_image;
        private BitmapImage m_bitmap;
        private int m_current;
        private readonly StripSlider m_slider;

        static Strip()
        {
            var owner = typeof(Strip);
            LinkProperty = DependencyProperty.Register(nameof(Link), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty, OnLinkChangedCallback));
            CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), owner, new FrameworkPropertyMetadata(0, OnCountChangedCallback));
            IntervalProperty = DependencyProperty.Register(nameof(Interval), typeof(int), owner, new FrameworkPropertyMetadata(1500, OnIntervalChangedCallback));
            CycleProperty = DependencyProperty.Register(nameof(Cycle), typeof(bool), owner, new FrameworkPropertyMetadata(true));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public Strip()
        {
            m_timer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromMilliseconds(Interval),
                IsEnabled = false
            };
            m_timer.Tick += OnTimer;

            m_slider = new StripSlider
            {
                Direction = SlideDirection.Left,
                Steps = 15,
                Interval = 50
            };
        }

        public bool Cycle
        {
            get => (bool)GetValue(CycleProperty);
            set => SetValue(CycleProperty, value);
        }

        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public string Link
        {
            get => (string)GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        public int Interval
        {
            get => (int)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_image = this.FindTemplatePart<ImageBrush>(PartImage)
                .AndIfNotNull(i =>
                {
                    i.Viewbox = new Rect(0, 0, 1, 1);
                    i.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
                });
            UpdateImage();
            OnIntervalChanged();
        }

        public void ResetCurrentIndex()
        {
            m_current = 0;
        }

        private static void OnLinkChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Strip control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnLinkChanged();
                }
            }
        }

        private static void OnCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Strip control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnCountChanged();
                }
            }
        }

        private static void OnIntervalChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Strip control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnIntervalChanged();
                }
            }
        }

        private void OnIntervalChanged()
        {
            m_timer.Stop();
            m_timer.Interval = TimeSpan.FromMilliseconds(Interval);
            if (Interval > 0)
            {
                m_timer.Start();
            }
        }

        private void OnCountChanged()
        {
            UpdateImage();
        }

        private void OnLinkChanged()
        {
            m_bitmap = new BitmapImage();
            if (!string.IsNullOrEmpty(Link))
            {
                m_bitmap.BeginInit();
                m_bitmap.UriSource = new Uri(Link, UriKind.Absolute);
                m_bitmap.EndInit();
            }
            UpdateImage();
        }

        private void UpdateImage()
        {
            m_current = 0;
            m_image.SetValueSafe(s => s.ImageSource, m_bitmap);
            if (m_bitmap != null && Count > 0)
            {
                m_slider.Slide(m_image, m_current, Count);
                m_current = 1;
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            if (CanMove(ref m_current))
            {
                m_slider.Slide(m_image, m_current, Count);
                m_current++;
            }
        }

        private bool CanMove(ref int current)
        {
            var result = m_bitmap != null && m_image != null && Count > 0;
            if (current >= Count)
            {
                if (Cycle)
                {
                    current = 0;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
