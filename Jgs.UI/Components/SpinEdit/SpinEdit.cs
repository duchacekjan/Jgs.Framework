using Jgs.UI.Components.SpinEditCore;
using Jgs.UI.Core;
using Jgs.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Jgs.UI.Components
{
    /// <summary>
    /// Komponenta pro nastavení a změnu hodnoty
    /// </summary>
    [TemplatePart(Name = PartUp, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PartDown, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = PartDisplayValue, Type = typeof(TextBox))]
    public class SpinEdit : Control
    {
        public const string PartUp = "PART_Up";
        public const string PartDown = "PART_Down";
        public const string PartEditor = "PART_Editor";
        public const string PartDisplayValue = "PART_DisplayValue";

        public const decimal DefaultStep = 1m;
        public const bool DefaultIsFloat = true;
        public static readonly string DefaultDisplayFormat = string.Empty;
        public static readonly string DefaultNullValueString = string.Empty;
        public static readonly decimal? DefaultMin = null;
        public static readonly decimal? DefaultMax = null;
        public static readonly decimal? DefaultValue = null;

        public static readonly DependencyProperty ValueProperty;
        public static readonly DependencyProperty NullValueStringProperty;
        public static readonly DependencyProperty IsFloatProperty;
        public static readonly DependencyProperty StepProperty;
        public static readonly DependencyProperty MinProperty;
        public static readonly DependencyProperty MaxProperty;
        public static readonly DependencyProperty DisplayFormatProperty;
        public static readonly DependencyProperty AllowCutProperty;
        public static readonly DependencyProperty NullValueProperty;
        public static readonly DependencyProperty ChangeOnMouseWheelProperty;

        private TextBox m_displayValue;
        private TextBox m_editor;
        private RepeatButton m_up;
        private RepeatButton m_down;
        private bool m_limitUpdating;
        private bool m_valueUpdating;
        private bool m_isDisplayValueMouseDown;
        private bool m_gotFocus;

        /// <summary>
        /// Statický konstruktor pro inicializaci dependency properties
        /// </summary>
        static SpinEdit()
        {
            var owner = typeof(SpinEdit);
            ValueProperty = DependencyProperty.Register(nameof(Value), typeof(decimal?), owner, new TwoWayPropertyMetadata(OnValuePropertyChangedCallback, OnValueCoerceValueCallback));
            MinProperty = DependencyProperty.Register(nameof(Min), typeof(decimal?), owner, new TwoWayPropertyMetadata(OnLimitValueChangedCallback));
            MaxProperty = DependencyProperty.Register(nameof(Max), typeof(decimal?), owner, new TwoWayPropertyMetadata(OnLimitValueChangedCallback));
            NullValueStringProperty = DependencyProperty.Register(nameof(NullValueString), typeof(string), owner, new FrameworkPropertyMetadata(DefaultNullValueString, OnValuePropertyChangedCallback));
            StepProperty = DependencyProperty.Register(nameof(Step), typeof(decimal), owner, new TwoWayPropertyMetadata(DefaultStep, OnStepPropertyChangedCallback));
            IsFloatProperty = DependencyProperty.Register(nameof(IsFloat), typeof(bool), owner, new FrameworkPropertyMetadata(DefaultIsFloat, OnLimitValueChangedCallback));
            DisplayFormatProperty = DependencyProperty.Register(nameof(DisplayFormat), typeof(string), owner, new FrameworkPropertyMetadata(DefaultDisplayFormat, OnValuePropertyChangedCallback));
            AllowCutProperty = DependencyProperty.Register(nameof(AllowCut), typeof(bool), owner, new FrameworkPropertyMetadata(false));
            NullValueProperty = DependencyProperty.Register(nameof(NullValue), typeof(decimal?), owner, new FrameworkPropertyMetadata(null, OnNullValuePropertyChangedCallback));
            ChangeOnMouseWheelProperty = DependencyProperty.Register(nameof(ChangeOnMouseWheel), typeof(bool), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public bool ChangeOnMouseWheel
        {
            get => (bool)GetValue(ChangeOnMouseWheelProperty);
            set => SetValue(ChangeOnMouseWheelProperty, value);
        }

        /// <summary>
        /// Hodnota reprezentující null hodnotu.
        /// Defaultní hodnota je <see langword="null"/>
        /// </summary>
        public decimal? NullValue
        {
            get => (decimal?)GetValue(NullValueProperty);
            set => SetValue(NullValueProperty, value);
        }

        /// <summary>
        /// Formát čísla.
        /// Defaultní hodnota je prázdný <see cref="string"/>
        /// </summary>
        public string DisplayFormat
        {
            get => (string)GetValue(DisplayFormatProperty);
            set => SetValue(DisplayFormatProperty, value);
        }

        /// <summary>
        /// Maximální povolená hodnota. Hodnota <see langword="null"/> znamená neomezeno.
        /// Defaultní hodnota je <see langword="null"/>
        /// </summary>
        public decimal? Max
        {
            get => (decimal?)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        /// <summary>
        /// Minimální povolená hodnota. Hodnota <see langword="null"/> znamená neomezeno.
        /// Defaultní hodnota je <see langword="null"/>
        /// </summary>
        public decimal? Min
        {
            get => (decimal?)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        /// <summary>
        /// Krok o který se bude zvyšovat nebo snižovat hodnota. Nesmí být menší než nula.
        /// Pokud bude, bude nastaven jako absolutní hodnota záporné hodnoty.
        /// Defaultní hodnota je <see langword="null"/>
        /// </summary>
        /// <remarks>
        /// Dojde-li ke změně <see cref="IsFloat"/> z <see langword="true"/> na <see langword="false"/> a krok
        /// je nastaven na desetinné číslo. Bude toto číslo zaokrouhleno směrem nahoru
        /// </remarks>
        public decimal Step
        {
            get => (decimal)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        /// <summary>
        /// Příznak, že může být zadáváno desetinné číslo.
        /// Defaultní hodnota je <see langword="true"/>
        /// </summary>
        public bool IsFloat
        {
            get => (bool)GetValue(IsFloatProperty);
            set => SetValue(IsFloatProperty, value);
        }

        /// <summary>
        /// Zástupný text pro hodnotu <see langword="null"/>.
        /// Defaultní hodnota je prázdný <see langword="string"/>
        /// </summary>
        public string NullValueString
        {
            get => (string)GetValue(NullValueStringProperty);
            set => SetValue(NullValueStringProperty, value);
        }

        /// <summary>
        /// Hodnota.
        /// Defaultní hodnota je <see langword="null"/>
        /// </summary>
        public decimal? Value
        {
            get => (decimal?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Příznak, zda je povoleno vyjmout hodnotu pomocí klávesové zkratky ctrl+x
        /// nebo přes možnosti kontextového menu.
        /// Defaultní hodnota je <see langword="false"/>
        /// </summary>
        public bool AllowCut
        {
            get => (bool)GetValue(AllowCutProperty);
            set => SetValue(AllowCutProperty, value);
        }

        /// <summary>When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.</summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_displayValue = this.FindTemplatePart<TextBox>(PartDisplayValue);
            m_editor = this.FindTemplatePart<TextBox>(PartEditor);
            m_up = this.FindTemplatePart<RepeatButton>(PartUp);
            m_down = this.FindTemplatePart<RepeatButton>(PartDown);
            InitializeDisplayValue();
            InitializeEditor();
            InitializeButton(m_up, true);
            InitializeButton(m_down, false);
        }

        /// <summary>
        /// Při pohybu kolečkem myši se vyvolá změna kroku (<see cref="Step"/>) daným směrem
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (ChangeOnMouseWheel)
            {
                if (e.Delta > 0)
                {
                    UpdateValue(1);
                }
                else
                {
                    UpdateValue(-1);
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Při obdržení focusu se ten přepošle na <see cref="PartEditor"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            StartEditing();
        }

        /// <summary>
        /// Pokud se myš ocitne uvnitř komponenty, zobrazí se cursor jako <see cref="Cursors.IBeam"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.IBeam;
        }

        /// <summary>
        /// Pokud myš opustí komponentu, nastaví se cursor zpět na default
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor = null;
        }

        /// <summary>
        /// Pokud kliknu na komponentu a editor nebude mít focus,
        /// tak se focusne editor
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (!m_editor.IsFocused)
            {
                StartEditing();
            }
        }

        /// <summary>
        /// Ošetření kláves.
        /// Šipka nahoru = zvýšit <see cref="Value"/> o <see cref="Step"/>
        /// Šipka dolů = snížit <see cref="Value"/> o <see cref="Step"/>
        /// Enter = ukončí editaci
        /// Escape = Ukončí editaci a zahodí nepotvrzené změny
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter when m_editor.IsFocused:
                    FinishEditing();
                    e.Handled = true;
                    break;
                case Key.Up:
                    UpdateValue(1);
                    e.Handled = true;
                    break;
                case Key.Down:
                    UpdateValue(-1);
                    e.Handled = true;
                    break;
                case Key.Left:
                case Key.Right:
                    e.Handled = false;
                    break;
                case Key.Escape:
                    FinishEditing(false);
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    if (!m_editor.IsFocused)
                    {
                        StartEditing();
                    }

                    break;
            }
        }

        /// <summary>
        /// Coerce callback při nastavení <see cref="Value"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="basevalue"></param>
        /// <returns></returns>
        private static object OnValueCoerceValueCallback(DependencyObject d, object basevalue)
        {
            var result = basevalue;
            if (d is SpinEdit control)
            {
                result = control.OnCoerceValue((decimal?)result);
            }

            return result;
        }

        /// <summary>
        /// Callback při změně <see cref="Value"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpinEdit control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnValueChanged();
                }
            }
        }

        /// <summary>
        /// Callback při změně vlastností, které ovlivňují limity.
        /// Jedná se o <see cref="Min"/>, <see cref="Max"/>, <see cref="IsFloat"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnLimitValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpinEdit control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnLimitValueChanged(e.Property.Name);
                }
            }
        }

        /// <summary>
        /// Callback při změně <see cref="Step"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnStepPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpinEdit control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnStepChanged();
                }
            }
        }

        /// <summary>
        /// Callback při změně <see cref="NullValue"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnNullValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpinEdit control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnNullValueChanged();
                }
            }
        }

        /// <summary>
        /// Metoda zakazující akci drag and drop copy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCopy(object sender, DataObjectCopyingEventArgs e)
        {
            if (e.IsDragDrop)
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Převede hodnotu na <see cref="string"/> pro zobrazení v <see cref="PartEditor"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ValueToEditor(decimal? value)
        {
            return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Připraví text pro převod na číslo.
        /// Začíná-li '-' upraví text na prázný <see cref="string"/>
        /// Končí-li oddělovačem desetin, doplní se za text 0
        /// Začíní-li oddělovačem desetin, doplní před text 0
        /// </summary>
        /// <param name="editorText"></param>
        /// <returns></returns>
        private static string AdjustText(string editorText)
        {
            var numberText = editorText;
            if (editorText == "-")
            {
                numberText = string.Empty;
            }

            if (editorText?.EndsWith(NumericAllowedCharsBuilder.DecimalSeparator) == true)
            {
                numberText += 0;
            }

            if (editorText?.StartsWith(NumericAllowedCharsBuilder.DecimalSeparator) == true)
            {
                numberText = 0 + numberText;
            }

            return numberText;
        }

        /// <summary>
        /// Připraví tlačítko. Napojí se na click event, pro úpravu <see cref="Value"/> pomocí  <see cref="Step"/>
        /// </summary>
        /// <param name="button"></param>
        /// <param name="isUp"></param>
        private void InitializeButton(ButtonBase button, bool isUp)
        {
            if (button != null)
            {
                var direction = isUp ? 1 : -1;
                button.Click += (s, e) =>
                {
                    UpdateValue(direction);
                };
            }
        }

        /// <summary>
        /// Připraví <see cref="PartDisplayValue"/> a napojí se na potřebné eventy
        /// </summary>
        private void InitializeDisplayValue()
        {
            if (m_displayValue != null)
            {
                RemoveDisplayValueHandlers();
                AddDisplayValueHandlers();
                SetDisplayValue(Value);
            }
        }

        /// <summary>
        /// Připraví <see cref="PartEditor"/> a napojí se na potřebné eventy
        /// </summary>
        private void InitializeEditor()
        {
            if (m_editor != null)
            {
                RemoveEditorHandlers();
                AddEditorHandlers();
                SetEditorValue(Value);
            }
        }

        /// <summary>
        /// Zruší napojení na eventy <see cref="PartEditor"/>
        /// </summary>
        private void RemoveEditorHandlers()
        {
            m_editor.LostFocus -= OnEditorLostFocus;
            m_editor.PreviewKeyDown -= OnEditorPreviewKeyDown;
            m_editor.PreviewTextInput -= OnEditorPreviewInput;
            m_editor.PreviewMouseDoubleClick -= OnEditorPreviewMouseDoubleClick;
            m_editor.TextChanged -= OnEditorTextChanged;

            DataObject.RemoveCopyingHandler(m_editor, OnCopy);
            DataObject.RemovePastingHandler(m_editor, OnPaste);
            CommandManager.RemovePreviewExecutedHandler(m_editor, OnCut);
        }

        /// <summary>
        /// Napojí eventy <see cref="PartEditor"/>
        /// </summary>
        private void AddEditorHandlers()
        {
            m_editor.LostFocus += OnEditorLostFocus;
            m_editor.PreviewKeyDown += OnEditorPreviewKeyDown;
            m_editor.PreviewTextInput += OnEditorPreviewInput;
            m_editor.PreviewMouseDoubleClick += OnEditorPreviewMouseDoubleClick;
            m_editor.TextChanged += OnEditorTextChanged;

            DataObject.AddPastingHandler(m_editor, OnPaste);
            DataObject.AddCopyingHandler(m_editor, OnCopy);
            CommandManager.AddPreviewExecutedHandler(m_editor, OnCut);
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!m_valueUpdating)
            {
                var numberText = AdjustText(m_editor.Text);
                if (decimal.TryParse(numberText, out var parsed))
                {
                    if (parsed > Max)
                    {
                        UpdateValueToLimit(Max);
                        e.Handled = true;
                    }

                    if (Min < 0 && parsed < Min)
                    {
                        UpdateValueToLimit(Min);
                        e.Handled = true;
                    }
                }
            }
        }

        private void UpdateValueToLimit(decimal? limit)
        {
            m_valueUpdating = true;
            var caret = m_editor.CaretIndex;
            if (m_editor.Text.Length != caret)
            {
                caret--;
            }

            Value = limit;
            SetEditorValue(Value);
            m_editor.CaretIndex = caret;
            m_valueUpdating = false;
        }

        /// <summary>
        /// Zruší napojení na eventy <see cref="PartDisplayValue"/>
        /// </summary>
        private void RemoveDisplayValueHandlers()
        {
            m_displayValue.PreviewMouseDoubleClick -= OnDisplayValueMouseUp;
            m_displayValue.PreviewMouseDown -= OnDisplayValuePreviewMouseDown;
            m_displayValue.MouseUp -= OnDisplayValueMouseUp;
            m_displayValue.GotKeyboardFocus -= OnDisplayValueGotKeyboardFocus;
        }

        /// <summary>
        /// Napojí se na eventy <see cref="PartDisplayValue"/>
        /// </summary>
        private void AddDisplayValueHandlers()
        {
            m_displayValue.PreviewMouseDoubleClick += OnDisplayValueMouseUp;
            m_displayValue.PreviewMouseDown += OnDisplayValuePreviewMouseDown;
            m_displayValue.PreviewMouseUp += OnDisplayValuePreviewMouseUp;
            m_displayValue.MouseUp += OnDisplayValueMouseUp;
            m_displayValue.GotKeyboardFocus += OnDisplayValueGotKeyboardFocus;
        }

        private void OnDisplayValuePreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(m_displayValue.SelectionStart);

            m_isDisplayValueMouseDown = false;
            if (m_gotFocus)
            {
                m_gotFocus = false;
                StartEditing();
            }
        }

        /// <summary>
        /// Při PreviewMouseDown na <see cref="PartDisplayValue"/> se poznačí, že bylo kliknuto
        /// a uloží se pozice caretu, která by měla být nastavena v <see cref="PartEditor"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDisplayValuePreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            m_isDisplayValueMouseDown = true;
        }

        /// <summary>
        /// Při uvolnění tlačíka myši na <see cref="PartDisplayValue"/> se poznačí, že myš byla uvolněna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDisplayValueMouseUp(object sender, MouseButtonEventArgs e)
        {
            m_isDisplayValueMouseDown = false;
            if (m_gotFocus)
            {
                m_gotFocus = false;
                StartEditing();
            }

            //e.Handled = true;
        }

        /// <summary>
        /// Při získání klávesového focusu na <see cref="PartDisplayValue"/> se spustí editace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDisplayValueGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!m_isDisplayValueMouseDown)
            {
                if (!IsKeyboardFocused && IsKeyboardFocusWithin && !m_valueUpdating)
                {
                    m_displayValue?.SelectAll();
                }

                StartEditing();
            }
            else
            {
                m_gotFocus = true;
            }
        }

        /// <summary>
        /// Pokud se zjistí DoubleClick na <see cref="PartEditor"/>, tak se označí celý text.
        /// Automatický dvojklik byl vždy přerušen oddělovačem desetin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditorPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (m_editor != null)
            {
                m_editor.SelectAll();
                e.Handled = true;
            }
        }

        /// <summary>
        /// V případě, že <see cref="PartEditor"/> ztratí focus, ukončí se editace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditorLostFocus(object sender, RoutedEventArgs e)
        {
            FinishEditing();
        }

        /// <summary>
        /// Metoda ošetřující vložení textu do vstupu (pomocí zkratky ctrl+v)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)) && m_editor != null)
            {
                var insertedText = e.DataObject.GetData(typeof(string)) as string;

                e.Handled = TryPaste(insertedText);
                e.CancelCommand();
            }
        }

        /// <summary>
        /// Metoda zakazující akci vyjmout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
            if (e?.Command == ApplicationCommands.Cut)
            {
                e.Handled = !AllowCut;
            }
        }

        /// <summary>
        /// V případě změny kroku se ověří, že je <see cref="Step"/> pozitivní. Pokud ne, použije se absolutní hodnota hodnoty.
        /// V případě, že se jená o desetinné číslo a <see cref="IsFloat"/> je nastaveno na <see langword="false"/>, tak dojde
        /// k zaokrouhlení na nejbližší vyšší celé číslo
        /// </summary>
        private void OnStepChanged()
        {
            //TODO Jak resit, jestli vubec, step = 0
            var value = Step;
            if (value < 0)
            {
                value = System.Math.Abs(value);
            }

            if (!IsFloat)
            {
                value = System.Math.Ceiling(value);
            }

            if (value != Step)
            {
                this.SetPropertyBackToModel(s => s.Step, value);
            }
        }

        /// <summary>
        /// Metoda se pokusí vložit text. Provede jeho validaci a případné označení vkládaného textu
        /// </summary>
        /// <param name="insertedText"></param>
        /// <returns></returns>
        private bool TryPaste(string insertedText)
        {
            var result = false;
            if (!string.IsNullOrEmpty(insertedText))
            {
                var replacedText = ReplaceInsertedText(insertedText, out var index, out var replacedLength);
                if (decimal.TryParse(replacedText, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out var number))
                {
                    Value = number;
                    m_editor.SelectionStart = index;
                    m_editor.SelectionLength = replacedLength;
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Nahradí vkládáný text jako nejjednodušší formu čísla. V out parametru <paramref name="index"/>
        /// se vrací pozice, kde bude začínat označení a v parametru <paramref name="replacedLength"/> délka
        /// textu, který bude vložen (délka výběru)
        /// </summary>
        /// <param name="insertedText"></param>
        /// <param name="index"></param>
        /// <param name="replacedLength"></param>
        /// <returns></returns>
        private string ReplaceInsertedText(string insertedText, out int index, out int replacedLength)
        {
            var result = m_editor.Text;
            var replacedText = insertedText.Replace(NumericAllowedCharsBuilder.ThousandsSeparator, string.Empty);
            replacedLength = replacedText.Length;
            var selectedText = m_editor.SelectedText;
            if (!string.IsNullOrEmpty(selectedText))
            {
                index = m_editor.SelectionStart;
                result = result.Replace(m_editor.SelectedText, replacedText);
            }
            else
            {
                index = m_editor.CaretIndex;
                result = result.Insert(m_editor.CaretIndex, replacedText);
            }

            return result;
        }

        /// <summary>
        /// Ošetření šipek nahoru a dolů pro úpravu <see cref="Value"/> od <see cref="Step"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditorPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    UpdateValue(1);
                    e.Handled = true;
                    break;
                case Key.Down:
                    UpdateValue(-1);
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        /// <summary>
        /// Ošetření změny limitů a pokud je potřeba tak i úprava hodnoty dle těchto limitů
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnLimitValueChanged(string propertyName)
        {
            if (!m_limitUpdating)
            {
                m_limitUpdating = true;

                HandleMinMax(propertyName);
                OnCoerceValue(Value);

                m_limitUpdating = false;
            }
        }

        /// <summary>
        /// Kontrola, že limity mají validní hodnoty vůči sobě
        /// </summary>
        /// <param name="propertyName"></param>
        private void HandleMinMax(string propertyName)
        {
            if (Min.HasValue && Max.HasValue)
            {
                switch (propertyName)
                {
                    case nameof(Min):
                        if (Min > Max)
                        {
                            Max = Min;
                        }

                        break;
                    case nameof(Max):
                        if (Max < Min)
                        {
                            Min = Max;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// V případě, že je <see cref="Value"/> nastavena na hodnotu <see langword="null"/>, dojde
        /// při změně <see cref="NullValue"/> k nastavení <see cref="Value"/> = <see cref="NullValue"/>
        /// </summary>
        private void OnNullValueChanged()
        {
            if (!Value.HasValue)
            {
                Value = NullValue;
            }
        }

        /// <summary>
        /// Kontrola, zda hodnota odpovídá zadným omezením
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private decimal? OnCoerceValue(decimal? value)
        {
            var result = HandleCoerceRange(value);
            result = HandleCoerceIsFloat(result);
#if NET48
            result = result ?? NullValue;
#else
            result ??= NullValue;
#endif

            if (value != result)
            {
                this.SetPropertyBackToModel(s => s.Value, result);
            }

            return result;
        }

        /// <summary>
        /// Kontrola hodnoty proti limitům <see cref="Min"/> a <see cref="Max"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private decimal? HandleCoerceRange(decimal? value)
        {
            if (Min.HasValue && value < Min)
            {
                value = Min;
            }

            if (Max.HasValue && value > Max)
            {
                value = Max;
            }

            return value;
        }

        /// <summary>
        /// Kontrola hodnoty proti nastavené vlastnosti <see cref="IsFloat"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private decimal? HandleCoerceIsFloat(decimal? value)
        {
            if (!IsFloat && value.HasValue)
            {
                OnStepChanged();
                value = System.Math.Round(value.Value);
            }

            return value;
        }

        /// <summary>
        /// Při změně hodnoty se nastaví text pro <see cref="PartDisplayValue"/> i pro <see cref="PartEditor"/>
        /// </summary>
        private void OnValueChanged()
        {
            if (!m_valueUpdating)
            {
                m_valueUpdating = true;
                SetDisplayValue(Value);
                SetEditorValue(Value);
                m_valueUpdating = false;
            }
        }

        /// <summary>
        /// Ošetření psaných znaků. Použije se <see cref="NumericAllowedCharsBuilder"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditorPreviewInput(object sender, TextCompositionEventArgs e)
        {
            var isAllowed = new NumericAllowedCharsBuilder()
                .WithTextToCompare(m_editor.Text.ReplaceFirst(m_editor.SelectedText, string.Empty))
                .WithDuplicityCheck()
                .WithDecimalSeparator(IsFloat)
                .WithMinusSign(!Min.HasValue || Min < 0, m_editor.CaretIndex)
                .IsAllowed(e.Text);
            e.Handled = !isAllowed;
        }

        /// <summary>
        /// Spustí editační režim. Skryje <see cref="PartDisplayValue"/>, zobrazí <see cref="PartEditor"/>
        /// a předá mu focus
        /// </summary>
        private void StartEditing()
        {
            if (m_valueUpdating)
            {
                m_valueUpdating = false;
                return;
            }

            UpdateSelection();

            m_displayValue.Visibility = Visibility.Collapsed;
            SetEditorValue(Value);
            m_editor.Visibility = Visibility.Visible;
            m_editor.Focus();
        }

        /// <summary>
        /// Označí text v <see cref="PartEditor"/>, dle označeného textu v <see cref="PartDisplayValue"/>
        /// </summary>
        private void UpdateSelection()
        {
            var start = GetEditorCaret();

            var displaySelection = m_displayValue.SelectedText
                .Replace(NumericAllowedCharsBuilder.ThousandsSeparator, string.Empty)
                .Length;

            m_editor.SelectionStart = start;
            m_editor.SelectionLength = displaySelection;
        }

        /// <summary>
        /// Převede caret z <see cref="PartDisplayValue"/> na caret <see cref="PartEditor"/>
        /// </summary>
        /// <returns></returns>
        private int GetEditorCaret()
        {
            var result = m_displayValue.SelectionStart;
            if (result != 0)
            {
                var part = result;

                result = m_displayValue.Text
                    .Substring(0, part)
                    .Replace(NumericAllowedCharsBuilder.ThousandsSeparator, string.Empty)
                    .Length;
            }

            return result;
        }

        /// <summary>
        /// Zruší editační režim. Skryje <see cref="PartEditor"/>, zobrazí <see cref="PartDisplayValue"/>
        /// a předá mu focus. V případě, že je <paramref name="confirmChange"/> nastaven na <see langword="true"/>,
        /// tak propíše změny do <see cref="Value"/> v opačném případě zahodí poslední změny
        /// </summary>
        /// <param name="confirmChange">Příznak, zda se má změna potvrdit nebo zrušit</param>
        private void FinishEditing(bool confirmChange = true)
        {
            if (confirmChange)
            {
                decimal? number = null;
                var numberText = AdjustText(m_editor.Text);
                if (decimal.TryParse(numberText, out var parsed))
                {
                    number = parsed;
                }

                Value = number;
            }

            SetEditorValue(Value);
            SetDisplayValue(Value);
            m_editor.Visibility = Visibility.Collapsed;
            m_displayValue.Visibility = Visibility.Visible;
            m_valueUpdating = true;
            m_displayValue.Focus();
        }

        /// <summary>
        /// Nastaví text do <see cref="PartDisplayValue"/> dle předané hodnoty
        /// </summary>
        /// <param name="value"></param>
        private void SetDisplayValue(decimal? value)
        {
            if (m_displayValue != null)
            {
                m_displayValue.Text = ValueToDisplayValue(value, DisplayFormat);
            }
        }

        /// <summary>
        /// Nastaví text to <see cref="PartEditor"/> dle předané hodnoty
        /// </summary>
        /// <param name="value"></param>
        private void SetEditorValue(decimal? value)
        {
            if (m_editor != null)
            {
                m_editor.Text = ValueToEditor(value);
            }
        }

        /// <summary>
        /// Převede hodnotu dle předaného formátu a v případě hodnoty <see langword="null"/>
        /// nastaví text na hodnotu uvedenou v <see cref="NullValueString"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="displayFormat"></param>
        /// <returns></returns>
        private string ValueToDisplayValue(decimal? value, string displayFormat)
        {
            return value?.ToString(displayFormat) ?? NullValueString;
        }

        /// <summary>
        /// Aktualizuje hodnotu daným směrem (zvýší = 1/ sníží = -1)
        /// </summary>
        /// <param name="direction"></param>
        private void UpdateValue(int direction)
        {
            m_valueUpdating = true;

            var updatedValue = GetUpdatedValue(Value, direction);

            if (updatedValue != Value)
            {
                Value = updatedValue;
                SetDisplayValue(Value);
                SetEditorValue(Value);
            }

            m_valueUpdating = false;
        }

        /// <summary>
        /// Provede úpravu hodnoty a zkontroluje, že odpovídá limitů
        /// </summary>
        /// <param name="value"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private decimal? GetUpdatedValue(decimal? value, int direction)
        {
            var updatedValue = value;
            if (updatedValue == null)
            {
                updatedValue = Min > 0 ? Min.Value : 0;
            }
            else
            {
                var step = direction * Step;
                updatedValue += step;
            }

            updatedValue = CheckRange(updatedValue);
            return updatedValue;
        }

        /// <summary>
        /// Zkontroluje zda hodnota nepřekračuje limity <see cref="Min"/> a <see cref="Max"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private decimal? CheckRange(decimal? value)
        {
            if (value < Min)
            {
                value = Min;
            }

            if (value > Max)
            {
                value = Max;
            }

            return value;
        }
    }
}