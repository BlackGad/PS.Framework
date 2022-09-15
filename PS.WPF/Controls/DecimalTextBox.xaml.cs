using System;
using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using PS.Extensions;
using PS.WPF.Automation;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class DecimalTextBox : BaseEditableBox,
                                  IValueProvider,
                                  IRangeValueProvider
    {
        public static readonly DependencyProperty EditableTextProperty =
            DependencyProperty.Register("EditableText",
                                        typeof(string),
                                        typeof(DecimalTextBox),
                                        new FrameworkPropertyMetadata(default(string), null, OnEditableTextPropertyCoerce));

        public static readonly DependencyProperty LargeChangeProperty =
            DependencyProperty.Register("LargeChange",
                                        typeof(decimal),
                                        typeof(DecimalTextBox),
                                        new FrameworkPropertyMetadata(new decimal(100), null, OnChangeCoerce));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum",
                                        typeof(decimal?),
                                        typeof(DecimalTextBox),
                                        new FrameworkPropertyMetadata(OnRangeChanged));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum",
                                        typeof(decimal?),
                                        typeof(DecimalTextBox),
                                        new FrameworkPropertyMetadata(OnRangeChanged));

        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register("Precision",
                                        typeof(int?),
                                        typeof(DecimalTextBox),
                                        new FrameworkPropertyMetadata(default(int?), OnPrecisionChanged));

        public static readonly DependencyProperty SmallChangeProperty =
            DependencyProperty.Register("SmallChange",
                                        typeof(decimal),
                                        typeof(DecimalTextBox),
                                        new FrameworkPropertyMetadata(new decimal(10), null, OnChangeCoerce));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value",
                                        typeof(decimal?),
                                        typeof(DecimalTextBox),
                                        new FrameworkPropertyMetadata(default(decimal?),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnValueChanged,
                                                                      OnValueCoerce));

        private static decimal? CoerceValue(decimal? source, decimal? maximum, decimal? minimum)
        {
            var value = source;

            if (value == null) return null;

            if (maximum.HasValue) value = Math.Min(value.Value, maximum.Value);
            if (minimum.HasValue) value = Math.Max(value.Value, minimum.Value);
            return value;
        }

        private static object OnChangeCoerce(DependencyObject d, object baseValue)
        {
            return Math.Max((decimal)baseValue, 0);
        }

        private static object OnEditableTextPropertyCoerce(DependencyObject d, object baseValue)
        {
            var owner = (DecimalTextBox)d;
            return owner.CoerceEditableText(baseValue as string);
        }

        private static void OnPrecisionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (DecimalTextBox)d;
            owner.UpdateDisplayText();
        }

        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (DecimalTextBox)d;
            var coercedValue = CoerceValue(owner.Value, owner.Maximum, owner.Minimum);
            if (!Equals(owner.Value, coercedValue)) owner.Value = coercedValue;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (DecimalTextBox)d;
            owner.NotifyValueChanged();
        }

        private static object OnValueCoerce(DependencyObject d, object baseValue)
        {
            var owner = (DecimalTextBox)d;

            if (baseValue == null) return null;

            var coercedValue = CoerceValue((decimal?)baseValue, owner.Maximum, owner.Minimum);

            if (coercedValue.AreEqual(owner.Value))
            {
                d.SetCurrentValue(ValueProperty, null);
                d.SetCurrentValue(ValueProperty, coercedValue);
            }

            return coercedValue;
        }

        private static decimal? ParseText(string text, CultureInfo cultureInfo)
        {
            if (decimal.TryParse(text, NumberStyles.Currency, cultureInfo, out var result)) return result;
            return null;
        }

        private System.Windows.Controls.TextBox _textBox;

        static DecimalTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalTextBox), new FrameworkPropertyMetadata(typeof(DecimalTextBox)));
            ResourceHelper.SetDefaultStyle(typeof(DecimalTextBox), Resource.ControlStyle);
        }

        public string EditableText
        {
            get { return (string)GetValue(EditableTextProperty); }
            set { SetValue(EditableTextProperty, value); }
        }

        public decimal LargeChange
        {
            get { return (decimal)GetValue(LargeChangeProperty); }
            set { SetValue(LargeChangeProperty, value); }
        }

        public decimal? Maximum
        {
            get { return (decimal?)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public decimal? Minimum
        {
            get { return (decimal?)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public int? Precision
        {
            get { return (int?)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        public decimal SmallChange
        {
            get { return (decimal)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }

        public decimal? Value
        {
            get { return (decimal?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        protected override void OnCancelEdit()
        {
            ClearValue(EditableTextProperty);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBox = GetTemplateChild("TextBox") as System.Windows.Controls.TextBox;
        }

        protected override string FormatValueToDisplayText(string formatString, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(formatString)) return FormatValue(Value);
            return Value?.ToString(formatString, CultureInfo);
        }

        protected override bool HasValue()
        {
            return Value != null;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new RelayElementAutomationPeer(this)
            {
                ControlType = AutomationControlType.Edit,
                ClassName = GetType().Name,
                GetPatternFunc = (patternInterface, owner) =>
                {
                    if (patternInterface == PatternInterface.Value || patternInterface == PatternInterface.RangeValue)
                    {
                        return this;
                    }

                    return null;
                }
            };
        }

        protected override void OnBeginEdit()
        {
            EditableText = ((IValueProvider)this).Value;

            if (_textBox != null)
            {
                _textBox.Focus();
                _textBox.SelectAll();
            }
        }

        protected override void OnEndEdit(EndEditReason reason)
        {
            ((IValueProvider)this).SetValue(EditableText);
            ClearValue(EditableTextProperty);
        }

        double IRangeValueProvider.LargeChange
        {
            get { return (double)LargeChange; }
        }

        double IRangeValueProvider.Maximum
        {
            get { return (double)(Maximum ?? decimal.MaxValue); }
        }

        double IRangeValueProvider.Minimum
        {
            get { return (double)(Minimum ?? decimal.MinValue); }
        }

        double IRangeValueProvider.SmallChange
        {
            get { return (double)SmallChange; }
        }

        double IRangeValueProvider.Value
        {
            get { return (double)(Value ?? 0); }
        }

        void IRangeValueProvider.SetValue(double value)
        {
            Value = (decimal)value;
        }

        string IValueProvider.Value
        {
            get { return FormatValue(Value); }
        }

        void IValueProvider.SetValue(string value)
        {
            Value = ParseText(value, CultureInfo);
        }

        protected object CoerceEditableText(string textValue)
        {
            var parsedValue = ParseText(textValue, CultureInfo);
            if (string.IsNullOrEmpty(textValue) || parsedValue.HasValue)
            {
                return textValue;
            }

            return EditableText;
        }

        protected string FormatValue(decimal? value)
        {
            if (!value.HasValue) return null;

            var format = "G";
            if (Precision.HasValue) format += Math.Max(0, Precision.Value);
            return value.Value.ToString(format, CultureInfo);
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/DecimalTextBox.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default DecimalTextBox style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default DecimalTextBox control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
