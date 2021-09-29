using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    [ContentProperty(nameof(Content))]
    public class KeyValueContentControl : HeaderedContentControl
    {
        #region Property definitions

        public static readonly DependencyProperty IsResizableProperty =
            DependencyProperty.RegisterAttached(nameof(IsResizable),
                                                typeof(bool),
                                                typeof(KeyValueContentControl),
                                                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty KeyColumnWidthProperty =
            DependencyProperty.RegisterAttached(nameof(KeyColumnWidth),
                                                typeof(double),
                                                typeof(KeyValueContentControl),
                                                new FrameworkPropertyMetadata(100d, OnKeyColumnWidthChanged));

        internal static readonly DependencyProperty KeyColumnGridLengthProperty =
            DependencyProperty.Register(nameof(KeyColumnGridLength),
                                        typeof(GridLength),
                                        typeof(KeyValueContentControl),
                                        new FrameworkPropertyMetadata(OnKeyColumnGridLengthChanged));

        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.Register(nameof(HeaderPadding),
                                        typeof(Thickness),
                                        typeof(KeyValueContentControl),
                                        new FrameworkPropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty KeyColumnWidthAdjustmentProperty =
            DependencyProperty.Register(nameof(KeyColumnWidthAdjustment),
                                        typeof(double),
                                        typeof(KeyValueContentControl),
                                        new FrameworkPropertyMetadata(OnKeyColumnWidthAdjustmentChanged));

        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
            DependencyProperty.Register(nameof(HorizontalHeaderAlignment),
                                        typeof(HorizontalAlignment),
                                        typeof(KeyValueContentControl),
                                        new FrameworkPropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty KeyContextMenuProperty =
            DependencyProperty.Register(nameof(KeyContextMenu),
                                        typeof(ContextMenu),
                                        typeof(KeyValueContentControl),
                                        new FrameworkPropertyMetadata(default(ContextMenu)));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation),
                                        typeof(Orientation),
                                        typeof(KeyValueContentControl),
                                        new FrameworkPropertyMetadata(default(Orientation)));

        public static readonly DependencyProperty VerticalHeaderAlignmentProperty =
            DependencyProperty.Register(nameof(VerticalHeaderAlignment),
                                        typeof(VerticalAlignment),
                                        typeof(KeyValueContentControl),
                                        new FrameworkPropertyMetadata(default(VerticalAlignment)));

        #endregion

        #region Static members

        public static bool? GetIsResizable(DependencyObject element)
        {
            if (element.IsDefaultValue(IsResizableProperty)) return null;
            return (bool?)element.GetValue(IsResizableProperty);
        }

        public static double? GetKeyColumnWidth(DependencyObject element)
        {
            if (element.IsDefaultValue(KeyColumnWidthProperty)) return null;
            return (double?)element.GetValue(KeyColumnWidthProperty);
        }

        public static void SetIsResizable(DependencyObject element, bool? value)
        {
            if (value == null)
            {
                element.ClearValue(IsResizableProperty);
            }
            else
            {
                element.SetValue(IsResizableProperty, value);
            }
        }

        public static void SetKeyColumnWidth(DependencyObject element, double? value)
        {
            if (value == null)
            {
                element.ClearValue(KeyColumnWidthProperty);
            }
            else
            {
                element.SetValue(KeyColumnWidthProperty, value);
            }
        }

        private static void OnKeyColumnGridLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (KeyValueContentControl)d;
            owner.OnKeyColumnGridLengthChanged(e);
        }

        private static void OnKeyColumnWidthAdjustmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (KeyValueContentControl)d;
            owner.OnKeyColumnWidthAdjustmentChanged(e);
        }

        private static void OnKeyColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KeyValueContentControl owner) owner.OnKeyColumnWidthChanged(e);
        }

        #endregion

        private bool _internalGridLengthChange;
        private bool _internalWidthChange;

        #region Constructors

        static KeyValueContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyValueContentControl), new FrameworkPropertyMetadata(typeof(KeyValueContentControl)));
            ResourceHelper.SetDefaultStyle(typeof(KeyValueContentControl), Resource.ControlStyle);
        }

        #endregion

        #region Properties

        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }

        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }

        public bool IsResizable
        {
            get { return (bool)GetValue(IsResizableProperty); }
            set { SetValue(IsResizableProperty, value); }
        }

        public double KeyColumnWidth
        {
            get { return (double)(GetValue(KeyColumnWidthProperty) ?? 0); }
            set { SetValue(KeyColumnWidthProperty, value); }
        }

        public double KeyColumnWidthAdjustment
        {
            get { return (double)GetValue(KeyColumnWidthAdjustmentProperty); }
            set { SetValue(KeyColumnWidthAdjustmentProperty, value); }
        }

        public ContextMenu KeyContextMenu
        {
            get { return (ContextMenu)GetValue(KeyContextMenuProperty); }
            set { SetValue(KeyContextMenuProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public VerticalAlignment VerticalHeaderAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalHeaderAlignmentProperty); }
            set { SetValue(VerticalHeaderAlignmentProperty, value); }
        }

        internal GridLength KeyColumnGridLength
        {
            get { return (GridLength)GetValue(KeyColumnGridLengthProperty); }
            set { SetValue(KeyColumnGridLengthProperty, value); }
        }

        #endregion

        #region Override members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetBindingToAncestorIfDefault(KeyColumnWidthProperty);
            this.SetBindingToAncestorIfDefault(IsResizableProperty);
        }

        #endregion

        #region Members

        private void InternalKeyColumnGridLengthUpdate(GridLength gridLength)
        {
            if (_internalWidthChange) return;

            try
            {
                _internalGridLengthChange = true;
                SetCurrentValue(KeyColumnWidthProperty, gridLength.Value - KeyColumnWidthAdjustment);
            }
            finally
            {
                _internalGridLengthChange = false;
            }
        }

        private void InternalKeyColumnWidthUpdate(double width)
        {
            if (_internalGridLengthChange) return;

            try
            {
                _internalWidthChange = true;
                SetCurrentValue(KeyColumnGridLengthProperty, new GridLength(width, GridUnitType.Pixel));
            }
            finally
            {
                _internalWidthChange = false;
            }
        }

        private void OnKeyColumnGridLengthChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is GridLength gridLength)
            {
                InternalKeyColumnGridLengthUpdate(gridLength);
            }
        }

        private void OnKeyColumnWidthAdjustmentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double adjustment)
            {
                InternalKeyColumnWidthUpdate(KeyColumnWidth + adjustment);
            }
        }

        private void OnKeyColumnWidthChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double width)
            {
                InternalKeyColumnWidthUpdate(width + KeyColumnWidthAdjustment);
            }
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/KeyValueContentControl.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default KeyValueContentControl style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default KeyValueContentControl control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}