using System;
using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using PS.WPF.Automation;
using PS.WPF.Patterns.Command;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class PasswordBox : BaseEditableBox,
                               IValueProvider

    {
        #region Property definitions

        public static readonly DependencyProperty SideButtonCommandProperty =
            DependencyProperty.Register(nameof(SideButtonCommand),
                                        typeof(IUICommand),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(IUICommand)));

        public static readonly DependencyProperty SideButtonStyleProperty =
            DependencyProperty.Register(nameof(SideButtonStyle),
                                        typeof(Style),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(Style)));

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(nameof(TextWrapping),
                                        typeof(TextWrapping),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(TextWrapping)));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value",
                                        typeof(string),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(string),
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnValueChanged));

        public static readonly DependencyProperty EditableTextProperty =
            DependencyProperty.Register("EditableText",
                                        typeof(string),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty IsDisplayTextSelectableProperty =
            DependencyProperty.Register("IsDisplayTextSelectable",
                                        typeof(bool),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register(nameof(IsPasswordVisible),
                                        typeof(bool),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(IsPasswordVisibleChanged));

        public static readonly DependencyProperty IsSideButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsSideButtonVisible),
                                        typeof(bool),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty SideButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(SideButtonCommandParameter),
                                        typeof(object),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty SideButtonGeometryProperty =
            DependencyProperty.Register(nameof(SideButtonGeometry),
                                        typeof(Geometry),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty SideButtonPasswordHiddenGeometryProperty =
            DependencyProperty.Register(nameof(SideButtonPasswordHiddenGeometry),
                                        typeof(Geometry),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty SideButtonPasswordVisibleGeometryProperty =
            DependencyProperty.Register(nameof(SideButtonPasswordVisibleGeometry),
                                        typeof(Geometry),
                                        typeof(PasswordBox),
                                        new FrameworkPropertyMetadata(default(Geometry)));

        #endregion

        #region Static members

        private static void IsPasswordVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (PasswordBox)d;
            owner.UpdateDisplayText();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (PasswordBox)d;
            owner.NotifyValueChanged();
        }

        #endregion

        private System.Windows.Controls.PasswordBox _passwordBox;
        private System.Windows.Controls.TextBox _textBox;

        #region Constructors

        static PasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(typeof(PasswordBox)));
            ResourceHelper.SetDefaultStyle(typeof(PasswordBox), Resource.ControlStyle);
        }

        public PasswordBox()
        {
            SetCurrentValue(SideButtonCommandProperty, new RelayUICommand(() => IsPasswordVisible = !IsPasswordVisible));
        }

        #endregion

        #region Properties

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

        public bool IsPasswordVisible
        {
            get { return (bool)GetValue(IsPasswordVisibleProperty); }
            set { SetValue(IsPasswordVisibleProperty, value); }
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

        public Geometry SideButtonPasswordHiddenGeometry
        {
            get { return (Geometry)GetValue(SideButtonPasswordHiddenGeometryProperty); }
            set { SetValue(SideButtonPasswordHiddenGeometryProperty, value); }
        }

        public Geometry SideButtonPasswordVisibleGeometry
        {
            get { return (Geometry)GetValue(SideButtonPasswordVisibleGeometryProperty); }
            set { SetValue(SideButtonPasswordVisibleGeometryProperty, value); }
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

        #endregion

        #region Override members

        protected override void OnBeginEdit()
        {
            EditableText = Value;

            if (_textBox != null)
            {
                _textBox.Focus();
                _textBox.SelectAll();
            }

            if (_passwordBox != null)
            {
                _passwordBox.Focus();
                _passwordBox.SelectAll();
            }
        }

        protected override void OnEndEdit(EndEditReason reason)
        {
            Value = EditableText;
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
            _passwordBox = GetTemplateChild("PasswordBox") as System.Windows.Controls.PasswordBox;
        }

        protected override string FormatValueToDisplayText(string formatString, CultureInfo cultureInfo)
        {
            var valueToDisplay = Value ?? string.Empty;
            if (!IsPasswordVisible)
            {
                valueToDisplay = string.Empty.PadRight(valueToDisplay.Length, '\u25CF');
            }

            //SideButtonGeometry
            return string.IsNullOrEmpty(valueToDisplay) ? valueToDisplay : string.Format(FormatString ?? "{0}", valueToDisplay);
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

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/PasswordBox.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default PasswordBox style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default PasswordBox control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor PasswordHiddenGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Default geometry for side button when password is hidden",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor PasswordVisibleGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Default geometry for side button when password is visible",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor SideButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default PasswordBox side button style",
                                                 resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}