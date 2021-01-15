using Jgs.Framework.UI.Markup;
using System;
using System.Collections;
using System.Resources;
using System.Windows;
using System.Windows.Controls;

namespace Jgs.Framework.UI.Components
{
    public class EnumValueSelector : ComboBox
    {
        public static readonly DependencyProperty EnumTypeProperty;
        public static readonly DependencyProperty TranslationManagerProperty;

        static EnumValueSelector()
        {
            var owner = typeof(EnumValueSelector);
            TranslationManagerProperty = DependencyProperty.Register(nameof(TranslationManager), typeof(ResourceManager), owner, new FrameworkPropertyMetadata(OnEnumTypeChangedCallback));
            EnumTypeProperty = DependencyProperty.Register(nameof(EnumType), typeof(Type), owner, new FrameworkPropertyMetadata(OnEnumTypeChangedCallback));
        }

        public EnumValueSelector()
        {
            DisplayMemberPath = "Value";
            SelectedValuePath = "Key";
        }

        public ResourceManager TranslationManager
        {
            get => (ResourceManager)GetValue(TranslationManagerProperty);
            set => SetValue(TranslationManagerProperty, value);
        }

        public Type EnumType
        {
            get => (Type)GetValue(EnumTypeProperty);
            set => SetValue(EnumTypeProperty, value);
        }

        private static void OnEnumTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EnumValueSelector control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnEnumTypeChanged();
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnEnumTypeChanged();
        }

        private void OnEnumTypeChanged()
        {
            if (EnumType != null)
            {
                var markup = new EnumValues
                {
                    EnumType = EnumType,
                    Resources = TranslationManager
                };

                ItemsSource = (IEnumerable)markup.ProvideValue(null);
            }
            else
            {
                ItemsSource = null;
            }
        }
    }
}
