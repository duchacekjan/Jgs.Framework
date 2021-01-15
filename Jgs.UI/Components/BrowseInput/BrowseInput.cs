using Jgs.UI.Components.BrowseInputCore;
using Jgs.UI.Extensions;
using Jgs.UI.FileOpenDialog;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using resx = Jgs.UI.Resources.Resources;

namespace Jgs.UI.Components
{ 
    public class BrowseInput : Control
    {
        public const string PartBrowse = "PART_Browse";
        public const string PartInput = "PART_Input";

        public static readonly DependencyProperty UserCanWriteProperty;
        public static readonly DependencyProperty IsSelectingFoldersProperty;
        public static readonly DependencyProperty IsMultiselectProperty;
        public static readonly DependencyProperty AddExtensionProperty;
        public static readonly DependencyProperty DereferenceLinksProperty;
        public static readonly DependencyProperty LabelProperty;
        public static readonly DependencyProperty FilterProperty;
        public static readonly DependencyProperty DialogTitleProperty;
        public static readonly DependencyProperty LabelPlacementProperty;
        public static readonly DependencyProperty BrowseButtonLabelProperty;
        public static readonly DependencyProperty BrowseButtonLabelTooltipProperty;
        public static readonly DependencyProperty SeparatorProperty;
        public static readonly DependencyProperty InitialDirProperty;

        public static readonly DependencyProperty FileNamesProperty;
        public static readonly DependencyProperty FileNameProperty;

        private TextBox m_input;
        static BrowseInput()
        {
            var owner = typeof(BrowseInput);
            UserCanWriteProperty = DependencyProperty.Register(nameof(UserCanWrite), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsSelectingFoldersProperty = DependencyProperty.Register(nameof(IsSelectingFolders), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsMultiselectProperty = DependencyProperty.Register(nameof(IsMultiselect), typeof(bool), owner, new FrameworkPropertyMetadata());
            DereferenceLinksProperty = DependencyProperty.Register(nameof(DereferenceLinks), typeof(bool), owner, new FrameworkPropertyMetadata());
            AddExtensionProperty = DependencyProperty.Register(nameof(AddExtension), typeof(bool), owner, new FrameworkPropertyMetadata());
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty));
            FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), owner, new FrameworkPropertyMetadata(resx.OpenDialogDefaultFilter));
            DialogTitleProperty = DependencyProperty.Register(nameof(DialogTitle), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty));
            BrowseButtonLabelProperty = DependencyProperty.Register(nameof(BrowseButtonLabel), typeof(string), owner, new FrameworkPropertyMetadata(resx.BrowseButtonLabel));
            BrowseButtonLabelTooltipProperty = DependencyProperty.Register(nameof(BrowseButtonLabelTooltip), typeof(string), owner, new FrameworkPropertyMetadata(resx.BrowseButtonLabelTooltip));
            LabelPlacementProperty = DependencyProperty.Register(nameof(LabelPlacement), typeof(BrowseInputLabelPlacement), owner, new FrameworkPropertyMetadata(BrowseInputLabelPlacement.Top));
            SeparatorProperty = DependencyProperty.Register(nameof(Separator), typeof(string), owner, new FrameworkPropertyMetadata(", ", OnSeparatorChangeCallback));
            InitialDirProperty = DependencyProperty.Register(nameof(InitialDir), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty));

            FileNamesProperty = DependencyProperty.Register(nameof(FileNames), typeof(IList<string>), owner, new FrameworkPropertyMetadata(OnFileNamesChangedCallback));
            FileNameProperty = DependencyProperty.Register(nameof(FileName), typeof(string), owner, new FrameworkPropertyMetadata(OnFileNameChangedCallback));

            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        private bool m_internalUpdate;

        public string InitialDir
        {
            get => (string)GetValue(InitialDirProperty);
            set => SetValue(InitialDirProperty, value);
        }

        public IList<string> FileNames
        {
            get => (IList<string>)GetValue(FileNamesProperty);
            set => SetValue(FileNamesProperty, value);
        }

        public string Separator
        {
            get => (string)GetValue(SeparatorProperty);
            set => SetValue(SeparatorProperty, value);
        }

        public string BrowseButtonLabel
        {
            get => (string)GetValue(BrowseButtonLabelProperty);
            set => SetValue(BrowseButtonLabelProperty, value);
        }

        public string BrowseButtonLabelTooltip
        {
            get => (string)GetValue(BrowseButtonLabelTooltipProperty);
            set => SetValue(BrowseButtonLabelTooltipProperty, value);
        }

        public bool IsMultiselect
        {
            get => (bool)GetValue(IsMultiselectProperty);
            set => SetValue(IsMultiselectProperty, value);
        }

        public bool IsSelectingFolders
        {
            get => (bool)GetValue(IsSelectingFoldersProperty);
            set => SetValue(IsSelectingFoldersProperty, value);
        }

        public bool UserCanWrite
        {
            get => (bool)GetValue(UserCanWriteProperty);
            set => SetValue(UserCanWriteProperty, value);
        }

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public bool DereferenceLinks
        {
            get => (bool)GetValue(DereferenceLinksProperty);
            set => SetValue(DereferenceLinksProperty, value);
        }

        public bool AddExtension
        {
            get => (bool)GetValue(AddExtensionProperty);
            set => SetValue(AddExtensionProperty, value);
        }

        public string DialogTitle
        {
            get => (string)GetValue(DialogTitleProperty);
            set => SetValue(DialogTitleProperty, value);
        }

        public BrowseInputLabelPlacement LabelPlacement
        {
            get => (BrowseInputLabelPlacement)GetValue(LabelPlacementProperty);
            set => SetValue(LabelPlacementProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var button = this.FindTemplatePart<Button>(PartBrowse)
                .AndIfNotNull(b =>
                {
                    b.Click += OnBrowseClick;
                });
            m_input = this.FindTemplatePart<TextBox>(PartInput);
        }

        private static void OnSeparatorChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BrowseInput control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnSeparatorChanged();
                }
            }
        }

        private static void OnFileNameChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is BrowseInput control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnFileNameChanged();
                }
            }

        }

        private static void OnFileNamesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is BrowseInput control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnFileNamesChanged();
                }
            }

        }

        private void OnFileNamesChanged()
        {
            if (!m_internalUpdate)
            {
                OnSeparatorChanged();
            }
        }

        private void OnFileNameChanged()
        {
            if (!m_internalUpdate)
            {
                FileNames = new List<string>
                {
                    FileName
                };
                OnSeparatorChanged();
            }
        }

        private void OnSeparatorChanged()
        {
            m_input.SetValueSafe(s => s.Text, string.Join(Separator, FileNames));
            if (FileNames.Count > 0)
            {
                m_input.SetValueSafe(s => s.ToolTip, CreateTextBoxTooltip());
            }
        }

        private void OnBrowseClick(object sender, RoutedEventArgs e)
        {
            var options = OpenDialogOptions.None;
            if (IsMultiselect)
            {
                options |= OpenDialogOptions.Multiselect;
            }

            if (IsSelectingFolders)
            {
                options |= OpenDialogOptions.PickFolders;
            }

            if (AddExtension)
            {
                options |= OpenDialogOptions.AddExtension;
            }

            if (DereferenceLinks)
            {
                options |= OpenDialogOptions.DereferenceLinks;
            }

            var dlg = new OpenFileDialog
            {
                Options = options,
                Title = DialogTitle,
                Filter = Filter,
                InitialDirectory = InitialDir
            };

            var hwnd = this.GetHWND();
            m_input.SetValueSafe(s => s.ToolTip, null);
            if (dlg.ShowDialog(hwnd))
            {
                m_internalUpdate = true;
                FileNames = dlg.FileNames;
                FileName = dlg.FileName;
                OnSeparatorChanged();
                m_internalUpdate = false;
            }
        }

        private ToolTip CreateTextBoxTooltip()
        {
            var tooltip =  new ToolTip()
            {
                Content = new ItemsControl
                {
                    ItemsSource = FileNames
                },
                PlacementTarget = m_input,
                Placement = PlacementMode.Custom
            };

            tooltip.CustomPopupPlacementCallback = OnTooltipCustom;
            return tooltip;
        }

        private CustomPopupPlacement[] OnTooltipCustom(Size popupSize, Size targetSize, Point offset)
        {
            return new CustomPopupPlacement[] 
            {
                new CustomPopupPlacement(new Point(-1, targetSize.Height), PopupPrimaryAxis.Vertical)
            };
        }
    }
}