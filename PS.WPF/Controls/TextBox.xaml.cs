using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using PS.Extensions;
using PS.WPF.Automation;
using PS.WPF.Patterns.Command;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class TextBox : BaseEditableBox,
                           IValueProvider

    {
        #region Property definitions

        public static readonly DependencyProperty SideButtonCommandProperty =
            DependencyProperty.Register(nameof(SideButtonCommand),
                                        typeof(IUICommand),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(IUICommand)));

        public static readonly DependencyProperty SideButtonStyleProperty =
            DependencyProperty.Register(nameof(SideButtonStyle),
                                        typeof(Style),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(Style)));

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(nameof(TextWrapping),
                                        typeof(TextWrapping),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(TextWrapping)));

        public static readonly RoutedEvent EditableTextChangedEvent =
            EventManager.RegisterRoutedEvent("EditableTextChanged",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(TextBox));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value",
                                        typeof(string),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(string),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnValueChanged));

        public static readonly DependencyProperty EditableTextProperty =
            DependencyProperty.Register("EditableText",
                                        typeof(string),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(OnEditableTextChanged, OnEditableTextCoerce));

        public static readonly DependencyProperty InputMatchPatternProperty =
            DependencyProperty.Register(nameof(InputMatchPattern),
                                        typeof(string),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty InputReplacementPatternProperty =
            DependencyProperty.Register(nameof(InputReplacementPattern),
                                        typeof(string),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty IsDisplayTextSelectableProperty =
            DependencyProperty.Register("IsDisplayTextSelectable",
                                        typeof(bool),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsSideButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsSideButtonVisible),
                                        typeof(bool),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty RealTimeValueChangeProperty =
            DependencyProperty.Register(nameof(RealTimeValueChange),
                                        typeof(bool),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty SideButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(SideButtonCommandParameter),
                                        typeof(object),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty SideButtonGeometryProperty =
            DependencyProperty.Register(nameof(SideButtonGeometry),
                                        typeof(Geometry),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty UseWatermarkWhenValueEmptyProperty =
            DependencyProperty.Register(nameof(UseWatermarkWhenValueEmpty),
                                        typeof(bool),
                                        typeof(TextBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        #endregion

        #region Constants

        #endregion

        #region Static members

        private static void OnEditableTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (TextBox)d;
            owner.RaiseEditableTextChangedEvent();
            if (owner.RealTimeValueChange && owner.IsEditMode)
            {
                owner.Value = owner.EditableText;
            }
        }

        private static object OnEditableTextCoerce(DependencyObject d, object baseValue)
        {
            var owner = (TextBox)d;
            if (baseValue is string input && !string.IsNullOrWhiteSpace(owner.InputMatchPattern))
            {
                if (string.IsNullOrWhiteSpace(owner.InputReplacementPattern))
                {
                    var matches = Regex.Matches(input, owner.InputMatchPattern)
                                       .Enumerate<Match>()
                                       .Where(m => m.Success)
                                       .Select(m => m.Value);

                    return string.Join(string.Empty, matches);
                }

                return Regex.Replace(input, owner.InputMatchPattern, owner.InputReplacementPattern);
            }

            return baseValue;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (TextBox)d;
            owner.NotifyValueChanged();
        }

        #endregion

        private string _lastValue;

        private System.Windows.Controls.TextBox _textBox;

        #region Constructors

        static TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
            ResourceHelper.SetDefaultStyle(typeof(TextBox), Resource.ControlStyle);
        }

        public TextBox()
        {
            SetCurrentValue(SideButtonCommandProperty,
                            new RelayUICommand(() =>
                            {
                                Value = null;
                                EndEdit();
                            }));
        }

        #endregion

        #region Properties

        public string EditableText
        {
            get { return (string)GetValue(EditableTextProperty); }
            set { SetValue(EditableTextProperty, value); }
        }

        public string InputMatchPattern
        {
            get { return (string)GetValue(InputMatchPatternProperty); }
            set { SetValue(InputMatchPatternProperty, value); }
        }

        public string InputReplacementPattern
        {
            get { return (string)GetValue(InputReplacementPatternProperty); }
            set { SetValue(InputReplacementPatternProperty, value); }
        }

        public bool IsDisplayTextSelectable
        {
            get { return (bool)GetValue(IsDisplayTextSelectableProperty); }
            set { SetValue(IsDisplayTextSelectableProperty, value); }
        }

        public bool IsSideButtonVisible
        {
            get { return (bool)GetValue(IsSideButtonVisibleProperty); }
            set { SetValue(IsSideButtonVisibleProperty, value); }
        }

        public bool RealTimeValueChange
        {
            get { return (bool)GetValue(RealTimeValueChangeProperty); }
            set { SetValue(RealTimeValueChangeProperty, value); }
        }

        public IUICommand SideButtonCommand
        {
            get { return (IUICommand)GetValue(SideButtonCommandProperty); }
            set { SetValue(SideButtonCommandProperty, value); }
        }

        public object SideButtonCommandParameter
        {
            get { return GetValue(SideButtonCommandParameterProperty); }
            set { SetValue(SideButtonCommandParameterProperty, value); }
        }

        public Geometry SideButtonGeometry
        {
            get { return (Geometry)GetValue(SideButtonGeometryProperty); }
            set { SetValue(SideButtonGeometryProperty, value); }
        }

        public Style SideButtonStyle
        {
            get { return (Style)GetValue(SideButtonStyleProperty); }
            set { SetValue(SideButtonStyleProperty, value); }
        }

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public bool UseWatermarkWhenValueEmpty
        {
            get { return (bool)GetValue(UseWatermarkWhenValueEmptyProperty); }
            set { SetValue(UseWatermarkWhenValueEmptyProperty, value); }
        }

        #endregion

        #region Events

        public event RoutedEventHandler EditableTextChanged
        {
            add { AddHandler(EditableTextChangedEvent, value); }
            remove { RemoveHandler(EditableTextChangedEvent, value); }
        }

        #endregion

        #region Override members

        protected override void OnBeginEdit()
        {
            _lastValue = Value;
            if (UseWatermarkWhenValueEmpty && IsWatermarkAvailable)
            {
                EditableText = Watermark;
            }
            else
            {
                EditableText = Value;
            }

            if (_textBox != null)
            {
                _textBox.Focus();
                _textBox.SelectAll();
            }
        }

        protected override void OnCancelEdit()
        {
            Value = _lastValue;
            ClearValue(EditableTextProperty);
        }

        protected override void OnEndEdit(EndEditReason reason)
        {
            Value = EditableText;
            ClearValue(EditableTextProperty);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new RelayElementAutomationPeer(this)
            {
                ControlType = AutomationControlType.Edit,
                ClassName = GetType().Name,
                GetPatternFunc = (patternInterface, owner) => patternInterface == PatternInterface.Value ? this : null
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBox = GetTemplateChild("TextBox") as System.Windows.Controls.TextBox;
        }

        protected override string FormatValueToDisplayText(string formatString, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(Value))
            {
                return Value;
            }

            if (string.IsNullOrEmpty(FormatString))
            {
                return Value;
            }

            return string.Format(FormatString, Value);
        }

        protected override bool HasValue()
        {
            return !string.IsNullOrEmpty(Value);
        }

        #endregion

        #region IValueProvider Members

        void IValueProvider.SetValue(string value)
        {
            Value = value;
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region Members

        protected virtual void RaiseEditableTextChangedEvent()
        {
            RaiseEvent(new RoutedEventArgs(EditableTextChangedEvent, this));
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/TextBox.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default TextBox style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default TextBox control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor SideButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default TextBox side button style",
                                                 resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}