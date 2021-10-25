using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.RoutedEvents;

namespace PS.WPF.Controls
{
    public abstract class BaseEditableBox : Control,
                                            IEditableObject
    {
        #region Property definitions

        private static readonly DependencyPropertyKey IsEditModePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsEditMode),
                                                typeof(bool),
                                                typeof(BaseEditableBox),
                                                new FrameworkPropertyMetadata(default(bool)));

        private static readonly DependencyPropertyKey IsWatermarkAvailablePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsWatermarkAvailable),
                                                typeof(bool),
                                                typeof(BaseEditableBox),
                                                new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty CultureInfoProperty =
            DependencyProperty.Register("CultureInfo",
                                        typeof(CultureInfo),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(OnCultureInfoChanged, OnCultureInfoCoerce));

        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText",
                                        typeof(string),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty FocusBorderVisibilityProperty =
            DependencyProperty.Register("FocusBorderVisibility",
                                        typeof(Visibility),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty FormatStringProperty =
            DependencyProperty.Register("FormatString",
                                        typeof(string),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(OnFormatStringChanged));

        public static readonly DependencyProperty HandleArrowsAsTabsProperty =
            DependencyProperty.Register("HandleArrowsAsTabs",
                                        typeof(bool),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsEditModeProperty = IsEditModePropertyKey.DependencyProperty;

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly",
                                        typeof(bool),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsWatermarkAvailableProperty = IsWatermarkAvailablePropertyKey.DependencyProperty;

        public static readonly DependencyProperty ManualEditModeProperty =
            DependencyProperty.Register("ManualEditMode",
                                        typeof(bool),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty RequireConfirmationOnFocusLostProperty =
            DependencyProperty.Register(nameof(RequireConfirmationOnFocusLost),
                                        typeof(bool),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment",
                                        typeof(TextAlignment),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata(default(TextAlignment)));

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark",
                                        typeof(string),
                                        typeof(BaseEditableBox),
                                        new FrameworkPropertyMetadata("Empty", OnWatermarkChanged));

        #endregion

        #region Constants

        public static readonly RoutedEvent EditCancelledEvent = EventManager.RegisterRoutedEvent(
            "EditCancelled",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(BaseEditableBox));

        public static readonly RoutedEvent EditCommittedEvent = EventManager.RegisterRoutedEvent(
            "EditCommitted",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler<EditCommittedEventArgs>),
            typeof(BaseEditableBox));

        public static readonly RoutedEvent EditStartedEvent = EventManager.RegisterRoutedEvent(
            "EditStarted",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(BaseEditableBox));

        #endregion

        #region Static members

        private static void OnCultureInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BaseEditableBox)d;
            owner.UpdateDisplayText();
        }

        private static object OnCultureInfoCoerce(DependencyObject d, object baseValue)
        {
            return baseValue ?? Thread.CurrentThread.CurrentUICulture;
        }

        private static void OnFormatStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BaseEditableBox)d;
            owner.UpdateDisplayText();
        }

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BaseEditableBox)d;
            owner.UpdateWatermarkAvailability();
        }

        #endregion

        #region Constructors

        protected BaseEditableBox()
        {
            UpdateWatermarkAvailability();
        }

        #endregion

        #region Properties

        public CultureInfo CultureInfo
        {
            get { return (CultureInfo)GetValue(CultureInfoProperty); }
            set { SetValue(CultureInfoProperty, value); }
        }

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public Visibility FocusBorderVisibility
        {
            get { return (Visibility)GetValue(FocusBorderVisibilityProperty); }
            set { SetValue(FocusBorderVisibilityProperty, value); }
        }

        public string FormatString
        {
            get { return (string)GetValue(FormatStringProperty); }
            set { SetValue(FormatStringProperty, value); }
        }

        public bool HandleArrowsAsTabs
        {
            get { return (bool)GetValue(HandleArrowsAsTabsProperty); }
            set { SetValue(HandleArrowsAsTabsProperty, value); }
        }

        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            private set { SetValue(IsEditModePropertyKey, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public bool IsWatermarkAvailable
        {
            get { return (bool)GetValue(IsWatermarkAvailableProperty); }
            private set { SetValue(IsWatermarkAvailablePropertyKey, value); }
        }

        public bool ManualEditMode
        {
            get { return (bool)GetValue(ManualEditModeProperty); }
            set { SetValue(ManualEditModeProperty, value); }
        }

        public bool RequireConfirmationOnFocusLost
        {
            get { return (bool)GetValue(RequireConfirmationOnFocusLostProperty); }
            set { SetValue(RequireConfirmationOnFocusLostProperty, value); }
        }

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        #endregion

        #region Events

        public event RoutedEventHandler EditCancelled
        {
            add { AddHandler(EditCancelledEvent, value); }
            remove { RemoveHandler(EditCancelledEvent, value); }
        }

        public event RoutedEventHandler<EditCommittedEventArgs> EditCommitted
        {
            add { AddHandler(EditCommittedEvent, value); }
            remove { RemoveHandler(EditCommittedEvent, value); }
        }

        public event RoutedEventHandler EditStarted
        {
            add { AddHandler(EditStartedEvent, value); }
            remove { RemoveHandler(EditStartedEvent, value); }
        }

        #endregion

        #region Override members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateDisplayText();
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            try
            {
                if (ManualEditMode) return;

                if (e.NewFocus is DependencyObject newFocusElement && newFocusElement.AreDiffers(this))
                {
                    if (this.HasVisualParent(newFocusElement)) return;

                    var parentPopup = newFocusElement.FindVisualParentOf<Popup>();
                    if (parentPopup != null && this.HasVisualParent(parentPopup.PlacementTarget)) return;

                    if (RequireConfirmationOnFocusLost)
                    {
                        CancelEdit();
                    }
                    else
                    {
                        EndEdit(EndEditReason.Focus);
                    }
                }
                else
                {
                    EndEdit(EndEditReason.Focus);
                }
            }
            finally
            {
                base.OnLostKeyboardFocus(e);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    if (IsEditMode)
                    {
                        CancelEdit();
                        e.Handled = true;
                    }

                    break;
                case Key.Enter:
                    EndEdit(EndEditReason.ReturnPressed);
                    break;
                case Key.Up:
                    if (HandleArrowsAsTabs)
                    {
                        if (Keyboard.FocusedElement is FrameworkElement element)
                        {
                            element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                        }

                        e.Handled = true;
                    }

                    break;
                case Key.Down:
                    if (HandleArrowsAsTabs)
                    {
                        if (Keyboard.FocusedElement is FrameworkElement element)
                        {
                            element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        }

                        e.Handled = true;
                    }

                    break;
                default:
                    if (ManualEditMode) return;
                    if (Keyboard.Modifiers != ModifierKeys.None) return;
                    if (e.Key >= Key.D0 && e.Key <= Key.Z)
                    {
                        BeginEdit();
                    }

                    break;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (ManualEditMode) return;

            Dispatcher.Postpone(BeginEdit);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);

            if (ManualEditMode) return;

            var oldObject = (UIElement)e.OldFocus;
            var newObject = (UIElement)e.NewFocus;
            if (!ReferenceEquals(newObject, this)) return;

            if (oldObject == null || !this.HasVisualParent(oldObject))
            {
                Dispatcher.Postpone(BeginEdit);
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.Tab) && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    var request = new TraversalRequest(FocusNavigationDirection.Previous);
                    MoveFocus(request);
                }
            }
        }

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
            if (IsEditMode || IsReadOnly) return;

            IsEditMode = true;

            OnBeginEdit();

            RaiseEditStartedEvent();
        }

        public void EndEdit()
        {
            EndEdit(EndEditReason.Manual);
        }

        public void CancelEdit()
        {
            if (IsEditMode)
            {
                IsEditMode = false;

                OnCancelEdit();

                RaiseEditCancelledEvent();
            }
        }

        #endregion

        #region Members

        protected void EndEdit(EndEditReason reason)
        {
            if (IsEditMode)
            {
                IsEditMode = false;
                OnEndEdit(reason);
                RaiseEditCommittedEvent(reason);
            }
        }

        protected abstract string FormatValueToDisplayText(string formatString, CultureInfo cultureInfo);

        protected abstract bool HasValue();

        protected void NotifyValueChanged()
        {
            UpdateDisplayText();
            UpdateWatermarkAvailability();
        }

        protected virtual void OnBeginEdit()
        {
        }

        protected virtual void OnCancelEdit()
        {
        }

        protected virtual void OnEndEdit(EndEditReason reason)
        {
        }

        protected virtual void RaiseEditCancelledEvent()
        {
            RaiseEvent(new RoutedEventArgs(EditCancelledEvent, this));
        }

        protected virtual void RaiseEditCommittedEvent(EndEditReason reason)
        {
            RaiseEvent(new EditCommittedEventArgs(reason, EditCommittedEvent, this));
        }

        protected virtual void RaiseEditStartedEvent()
        {
            RaiseEvent(new RoutedEventArgs(EditStartedEvent, this));
        }

        protected void UpdateDisplayText()
        {
            DisplayText = FormatValueToDisplayText(FormatString, CultureInfo);
        }

        protected void UpdateWatermarkAvailability()
        {
            IsWatermarkAvailable = !string.IsNullOrEmpty(Watermark) && !HasValue();
        }

        #endregion
    }

    public enum EndEditReason
    {
        Manual,
        Focus,
        ReturnPressed
    }

    public class EditCommittedEventArgs : RoutedEventArgs
    {
        #region Constructors

        public EditCommittedEventArgs(EndEditReason reason, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            Reason = reason;
        }

        #endregion

        #region Properties

        public EndEditReason Reason { get; }

        #endregion
    }
}