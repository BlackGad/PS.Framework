using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using PS.Extensions;
using PS.WPF.Automation;
using PS.WPF.Patterns.Command;
using PS.WPF.Resources;
using PS.WPF.ValueConverters;

namespace PS.WPF.Controls
{
    public class TimeSpanBox : BaseEditableBox,
                               IValueProvider

    {
        public static readonly DependencyProperty EditableTextProperty =
            DependencyProperty.Register("EditableText",
                                        typeof(string),
                                        typeof(TimeSpanBox),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty IsDisplayTextSelectableProperty =
            DependencyProperty.Register("IsDisplayTextSelectable",
                                        typeof(bool),
                                        typeof(TimeSpanBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsSideButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsSideButtonVisible),
                                        typeof(bool),
                                        typeof(TimeSpanBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty SideButtonCommandProperty =
            DependencyProperty.Register(nameof(SideButtonCommand),
                                        typeof(IUICommand),
                                        typeof(TimeSpanBox),
                                        new FrameworkPropertyMetadata(default(IUICommand)));

        public static readonly DependencyProperty SideButtonStyleProperty =
            DependencyProperty.Register(nameof(SideButtonStyle),
                                        typeof(Style),
                                        typeof(TimeSpanBox),
                                        new FrameworkPropertyMetadata(default(Style)));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value",
                                        typeof(TimeSpan?),
                                        typeof(TimeSpanBox),
                                        new FrameworkPropertyMetadata(default(TimeSpan?),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnValueChanged,
                                                                      OnValueCoerce));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (TimeSpanBox)d;
            owner.NotifyValueChanged();
        }

        private static object OnValueCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private System.Windows.Controls.TextBox _textBox;

        static TimeSpanBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSpanBox), new FrameworkPropertyMetadata(typeof(TimeSpanBox)));
            ResourceHelper.SetDefaultStyle(typeof(TimeSpanBox), Resource.ControlStyle);
        }

        public TimeSpanBox()
        {
            SideButtonCommand = new RelayUICommand(() =>
            {
                Value = null;
                if (IsEditMode) EditableText = string.Empty;
            });
        }

        public string EditableText
        {
            get { return (string)GetValue(EditableTextProperty); }
            set { SetValue(EditableTextProperty, value); }
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

        public IUICommand SideButtonCommand
        {
            get { return (IUICommand)GetValue(SideButtonCommandProperty); }
            set { SetValue(SideButtonCommandProperty, value); }
        }

        public Style SideButtonStyle
        {
            get { return (Style)GetValue(SideButtonStyleProperty); }
            set { SetValue(SideButtonStyleProperty, value); }
        }

        public TimeSpan? Value
        {
            get { return (TimeSpan?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        protected override void OnBeginEdit()
        {
            EditableText = Convert(Value);

            if (_textBox != null)
            {
                _textBox.Focus();
                _textBox.SelectAll();
            }
        }

        protected override void OnEndEdit(EndEditReason reason)
        {
            Value = Convert(EditableText);
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
            if (Value == null) return null;
            return string.IsNullOrEmpty(FormatString) ? Convert(Value) : string.Format(FormatString, Value);
        }

        protected override bool HasValue()
        {
            return Value != null;
        }

        void IValueProvider.SetValue(string value)
        {
            Value = Convert(value);
        }

        string IValueProvider.Value
        {
            get { return Convert(Value); }
        }

        protected virtual TimeSpan? Convert(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var milliseconds = 0;
            var seconds = 0;
            var minutes = 0;
            var hours = 0;
            var days = 0;

            var queue = new Queue<string>(value.Split(':', ';', '\\', '/').Reverse());

            if (queue.TryDequeue(out var secondsToken))
            {
                var secondsQueue = new Queue<string>(secondsToken.Split('.', ','));

                if (secondsQueue.TryDequeue(out var secondsString)) int.TryParse(secondsString, out seconds);
                if (secondsQueue.TryDequeue(out var millisecondsString) &&
                    TimeSpan.TryParse("00:00:00." + millisecondsString, out var millisecondsExtractor))
                {
                    milliseconds = (int)millisecondsExtractor.TotalMilliseconds;
                }
            }

            if (queue.TryDequeue(out var minutesString)) int.TryParse(minutesString, out minutes);
            if (queue.TryDequeue(out var hoursString)) int.TryParse(hoursString, out hours);
            if (queue.TryDequeue(out var daysString)) int.TryParse(daysString, out days);

            return new TimeSpan(days, hours, minutes, seconds, milliseconds);
        }

        protected virtual string Convert(TimeSpan? value)
        {
            return (string)ObjectConverters.Format.Convert(value,
                                                           typeof(string),
                                                           string.Empty,
                                                           CultureInfo.CurrentUICulture);
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/TimeSpanBox.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default TimeSpanBox style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default TimeSpanBox control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor SideButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default TimeSpanBox side button style",
                                                 resourceDictionary: Default);
        }

        #endregion
    }
}
