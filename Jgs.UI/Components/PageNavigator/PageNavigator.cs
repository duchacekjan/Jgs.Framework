using Jgs.UI.Components.PageNavigatorCore;
using Jgs.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jgs.UI.Components
{
    public class PageNavigator : Control
    {
        public const string PartCurrentPage = "PART_CurrentPage";
        public const string PartTotalPages = "PART_TotalPages";
        public const string PartFirst = "PART_First";
        public const string PartLast = "PART_Last";
        public const string PartPrevious = "PART_Previous";
        public const string PartNext = "PART_Next";
        public static readonly DependencyProperty CurrentPageProperty;
        public static readonly DependencyProperty TotalPagesProperty;
        public static readonly DependencyProperty CommandProperty;

        private TextBlock m_currentPage;
        private TextBlock m_totalPages;

        private PathButton m_first;
        private PathButton m_last;
        private PathButton m_previous;
        private PathButton m_next;

        static PageNavigator()
        {
            var owner = typeof(PageNavigator);
            CurrentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(int), owner, new FrameworkPropertyMetadata(0, OnPagesChangedCallback));
            TotalPagesProperty = DependencyProperty.Register(nameof(TotalPages), typeof(int), owner, new FrameworkPropertyMetadata(0, OnPagesChangedCallback));
            CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public int TotalPages
        {
            get => (int)GetValue(TotalPagesProperty);
            set => SetValue(TotalPagesProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_currentPage = this.FindTemplatePart<TextBlock>(PartCurrentPage);
            m_totalPages = this.FindTemplatePart<TextBlock>(PartTotalPages);
            m_first = GetButton(PartFirst, PageNavigatorCommand.First);
            m_last = GetButton(PartLast, PageNavigatorCommand.Last);
            m_previous = GetButton(PartPrevious, PageNavigatorCommand.Previous);
            m_next = GetButton(PartNext, PageNavigatorCommand.Next);
            OnPagesChanged();
        }

        private PathButton GetButton(string partName, PageNavigatorCommand command)
        {
            return this.FindTemplatePart<PathButton>(partName)
                .AndIfNotNull(b =>
                {
                    b.Click += (s, e) => OnClick(command);
                });
        }

        private void OnClick(PageNavigatorCommand command)
        {
            Command.ExecuteCmd(command);
        }

        private static void OnPagesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PageNavigator control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnPagesChanged();
                }
            }
        }

        private void OnPagesChanged()
        {
            if (CurrentPage > TotalPages || CurrentPage <= 0 || TotalPages <= 0)
            {
                m_currentPage.SetValueSafe(s => s.Text, "0");
                m_totalPages.SetValueSafe(s => s.Text, "0");
                m_currentPage.SetValueSafe(s => s.IsEnabled, false);
                m_totalPages.SetValueSafe(s => s.IsEnabled, false);
            }
            else
            {
                m_currentPage.SetValueSafe(s => s.Text, $"{CurrentPage}");
                m_totalPages.SetValueSafe(s => s.Text, $"{TotalPages}");
                m_currentPage.SetValueSafe(s => s.IsEnabled, true);
                m_totalPages.SetValueSafe(s => s.IsEnabled, true);
            }
            SetButtonsEnabled();
        }

        private void SetButtonsEnabled()
        {
            SetButtonEnabled(m_first, CurrentPage > 1);
            SetButtonEnabled(m_last, CurrentPage < TotalPages);
            SetButtonEnabled(m_previous, CurrentPage > 1);
            SetButtonEnabled(m_next, CurrentPage < TotalPages);
        }

        private void SetButtonEnabled(PathButton button, bool isEnabled)
        {
            button.SetValueSafe(b => b.IsEnabled, isEnabled);
        }
    }
}
