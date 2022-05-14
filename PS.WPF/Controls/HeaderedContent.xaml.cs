using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    [ContentProperty(nameof(Content))]
    public class HeaderedContent : HeaderedContentControl
    {
        #region Property definitions

        public static readonly DependencyProperty HeaderColumnWidthAdjustmentProperty =
            DependencyProperty.Register(nameof(HeaderColumnWidthAdjustment),
                                        typeof(double),
                                        typeof(HeaderedContent),
                                        new FrameworkPropertyMetadata(OnHeaderColumnWidthAdjustmentChanged));

        public static readonly DependencyProperty HeaderColumnWidthProperty =
            DependencyProperty.RegisterAttached(nameof(HeaderColumnWidth),
                                                typeof(double),
                                                typeof(HeaderedContent),
                                                new FrameworkPropertyMetadata(100d, OnHeaderColumnWidthChanged));

        public static readonly DependencyProperty HeaderContextMenuProperty =
            DependencyProperty.Register(nameof(HeaderContextMenu),
                                        typeof(ContextMenu),
                                        typeof(HeaderedContent),
                                        new FrameworkPropertyMetadata(default(ContextMenu)));

        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.Register(nameof(HeaderPadding),
                                        typeof(Thickness),
                                        typeof(HeaderedContent),
                                        new FrameworkPropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
            DependencyProperty.Register(nameof(HorizontalHeaderAlignment),
                                        typeof(HorizontalAlignment),
                                        typeof(HeaderedContent),
                                        new FrameworkPropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty IsResizableProperty =
            DependencyProperty.RegisterAttached(nameof(IsResizable),
                                                typeof(bool),
                                                typeof(HeaderedContent),
                                                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation),
                                        typeof(Orientation),
                                        typeof(HeaderedContent),
                                        new FrameworkPropertyMetadata(default(Orientation)));

        public static readonly DependencyProperty VerticalHeaderAlignmentProperty =
            DependencyProperty.Register(nameof(VerticalHeaderAlignment),
                                        typeof(VerticalAlignment),
                                        typeof(HeaderedContent),
                                        new FrameworkPropertyMetadata(default(VerticalAlignment)));

        internal static readonly DependencyProperty HeaderColumnGridLengthProperty =
            DependencyProperty.Register(nameof(HeaderColumnGridLength),
                                        typeof(GridLength),
                                        typeof(HeaderedContent),
                                        new FrameworkPropertyMetadata(OnHeaderColumnGridLengthChanged));

        #endregion

        #region Static members

        public static double? GetHeaderColumnWidth(DependencyObject element)
        {
            if (element.IsDefaultValue(HeaderColumnWidthProperty)) return null;
            return (double?)element.GetValue(HeaderColumnWidthProperty);
        }

        public static bool? GetIsResizable(DependencyObject element)
        {
            if (element.IsDefaultValue(IsResizableProperty)) return null;
            return (bool?)element.GetValue(IsResizableProperty);
        }

        public static void SetHeaderColumnWidth(DependencyObject element, double? value)
        {
            if (value == null)
            {
                element.ClearValue(HeaderColumnWidthProperty);
            }
            else
            {
                element.SetValue(HeaderColumnWidthProperty, value);
            }
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

        private static void OnHeaderColumnGridLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (HeaderedContent)d;
            owner.OnHeaderColumnGridLengthChanged(e);
        }

        private static void OnHeaderColumnWidthAdjustmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (HeaderedContent)d;
            owner.OnHeaderColumnWidthAdjustmentChanged(e);
        }

        private static void OnHeaderColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HeaderedContent owner) owner.OnHeaderColumnWidthChanged(e);
        }

        #endregion

        private bool _internalGridLengthChange;
        private bool _internalWidthChange;

        #region Constructors

        static HeaderedContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedContent), new FrameworkPropertyMetadata(typeof(HeaderedContent)));
            ResourceHelper.SetDefaultStyle(typeof(HeaderedContent), Resource.ControlStyle);
        }

        #endregion

        #region Properties

        public double HeaderColumnWidth
        {
            get { return (double)(GetValue(HeaderColumnWidthProperty) ?? 0); }
            set { SetValue(HeaderColumnWidthProperty, value); }
        }

        public double HeaderColumnWidthAdjustment
        {
            get { return (double)GetValue(HeaderColumnWidthAdjustmentProperty); }
            set { SetValue(HeaderColumnWidthAdjustmentProperty, value); }
        }

        public ContextMenu HeaderContextMenu
        {
            get { return (ContextMenu)GetValue(HeaderContextMenuProperty); }
            set { SetValue(HeaderContextMenuProperty, value); }
        }

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

        internal GridLength HeaderColumnGridLength
        {
            get { return (GridLength)GetValue(HeaderColumnGridLengthProperty); }
            set { SetValue(HeaderColumnGridLengthProperty, value); }
        }

        #endregion

        #region Override members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetBindingToAncestorIfDefault(HeaderColumnWidthProperty);
            this.SetBindingToAncestorIfDefault(IsResizableProperty);
        }

        #endregion

        #region Members

        private void InternalHeaderColumnGridLengthUpdate(GridLength gridLength)
        {
            if (_internalWidthChange) return;

            try
            {
                _internalGridLengthChange = true;
                SetCurrentValue(HeaderColumnWidthProperty, gridLength.Value - HeaderColumnWidthAdjustment);
            }
            finally
            {
                _internalGridLengthChange = false;
            }
        }

        private void InternalHeaderColumnWidthUpdate(double width)
        {
            if (_internalGridLengthChange) return;

            try
            {
                _internalWidthChange = true;
                SetCurrentValue(HeaderColumnGridLengthProperty, new GridLength(width, GridUnitType.Pixel));
            }
            finally
            {
                _internalWidthChange = false;
            }
        }

        private void OnHeaderColumnGridLengthChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is GridLength gridLength)
            {
                InternalHeaderColumnGridLengthUpdate(gridLength);
            }
        }

        private void OnHeaderColumnWidthAdjustmentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double adjustment)
            {
                InternalHeaderColumnWidthUpdate(HeaderColumnWidth + adjustment);
            }
        }

        private void OnHeaderColumnWidthChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double width)
            {
                InternalHeaderColumnWidthUpdate(width + HeaderColumnWidthAdjustment);
            }
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/HeaderedContent.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            #endregion
        }

        #endregion
    }
}