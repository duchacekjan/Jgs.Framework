using Jgs.UI.Components.TagControlCore;
using Jgs.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Jgs.UI.Components
{
    [ContentProperty(nameof(Text))]
    public class Tag : Control
    {
        public const string PartCloseButton = "PART_CloseButton";
        public const string PartBorder = "PART_Border";
        public const string PartViewBox = "PART_ViewBox";
        public static readonly DependencyProperty CommandProperty;
        public static readonly DependencyProperty CloseButtonVisibleProperty;
        public static readonly DependencyProperty IdProperty;
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty CloseButtonTooltipProperty;

        private bool m_buttonDown;
        private Button m_closeButton;
        private Viewbox m_viewbox;

        static Tag()
        {
            var owner = typeof(Tag);
            CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), owner, new FrameworkPropertyMetadata());
            CloseButtonVisibleProperty = DependencyProperty.Register(nameof(CloseButtonVisible), typeof(bool), owner, new FrameworkPropertyMetadata(true, OnCloseButtonVisiblePropertyChangedCallback));
            IdProperty = DependencyProperty.Register(nameof(Id), typeof(object), owner, new FrameworkPropertyMetadata());
            TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty));
            CloseButtonTooltipProperty = DependencyProperty.Register(nameof(CloseButtonTooltip), typeof(string), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public string CloseButtonTooltip
        {
            get => (string)GetValue(CloseButtonTooltipProperty);
            set => SetValue(CloseButtonTooltipProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public object Id
        {
            get => GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public bool CloseButtonVisible
        {
            get => (bool)GetValue(CloseButtonVisibleProperty);
            set => SetValue(CloseButtonVisibleProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_closeButton = this.FindTemplatePart<Button>(PartCloseButton)
                .AndIfNotNull(b =>
                {
                    b.Click += (s, e) => InvokeCmd(TagCommmandSource.CloseButton);
                });
            this.FindTemplatePart<Border>(PartBorder)
                .IfNotNull(b =>
                {
                    b.MouseDown += OnBorderMouseDown;
                    b.MouseUp += OnBorderMouseUp;
                });
            m_viewbox = this.FindTemplatePart<Viewbox>(PartViewBox);
        }

        private static void OnCloseButtonVisiblePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Tag control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnCloseButtonVisiblePropertyChanged();
                }
            }
        }

        private void OnCloseButtonVisiblePropertyChanged()
        {
            var visibility = Visibility.Collapsed;
            var margin = new Thickness(8, 2, 8, 2);
            if (CloseButtonVisible)
            {
                visibility = Visibility.Visible;
                margin = new Thickness(8, 2, 4, 2);
            }

            m_closeButton.SetValueSafe(s => s.Visibility, visibility);
            m_viewbox.SetValueSafe(s => s.Margin, margin);
        }

        private void OnBorderMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released && m_buttonDown)
            {
                m_buttonDown = false;
                InvokeCmd(TagCommmandSource.Tag);
            }
        }

        private void OnBorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            m_buttonDown = e.MiddleButton == MouseButtonState.Pressed;
        }

        private void InvokeCmd(TagCommmandSource source)
        {
            var args = new TagCommand(Id, source);
            if (Command?.CanExecute(args) == true)
            {
                Command.Execute(args);
            }
        }
    }
}
