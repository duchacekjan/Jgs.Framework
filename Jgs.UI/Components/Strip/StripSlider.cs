using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Jgs.UI.Components.StripCore
{
    public class StripSlider
    {
        private readonly DispatcherTimer m_timer;
        private Func<bool> m_slider;
        private int m_counter;

        public StripSlider()
        {
            m_timer = new DispatcherTimer(DispatcherPriority.Render);
            m_timer.Tick += OnTimer;
        }

        public SlideDirection Direction { get; set; }

        public double Steps { get; set; }

        public int Interval { get; set; }

        private bool IsHorizontalSlide => Direction == SlideDirection.Left || Direction == SlideDirection.Right;

        private int Sign => Direction == SlideDirection.Right || Direction == SlideDirection.Up ? -1 : 1;

        public void Slide(TileBrush brush, double newIndex, double stripCount)
        {
            m_timer.Stop();
            if (brush != null)
            {
                m_timer.Interval = TimeSpan.FromMilliseconds(Interval);
                var size = 1.0 / stripCount;
                var rectSize = GetSize(size);
                if (newIndex > 0)
                {
                    m_counter = 0;
                    var start = (newIndex - 1) * size;
                    var end = newIndex * size;
                    var step = (end - start) / Steps;

                    m_slider = () =>
                    {
                        var move = (start + m_counter * step) * Sign;
                        var point = GetPoint(move);
                        brush.Viewbox = new Rect(point, rectSize);
                        m_counter++;
                        return m_counter > Steps;
                    };
                    m_timer.Start();
                }
                else
                {
                    var point = GetPoint(0);
                    brush.Viewbox = new Rect(point, rectSize);
                }
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            if (m_slider?.Invoke() == true)
            {
                m_timer.Stop();
            }
        }

        private Point GetPoint(double move)
        {
            return IsHorizontalSlide
                ? new Point(move, 0)
                : new Point(0, move);
        }

        private Size GetSize(double size)
        {
            return IsHorizontalSlide
                ? new Size(size, 1)
                : new Size(1, size);
        }

    }
}
