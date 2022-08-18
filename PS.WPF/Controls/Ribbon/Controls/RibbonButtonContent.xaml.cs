using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonButtonContent : Control
    {
        public static readonly DependencyProperty HasTwoLinesProperty =
            DependencyProperty.Register(nameof(HasTwoLines),
                                        typeof(bool),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register(nameof(ImageSize),
                                        typeof(RibbonImageSize),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(RibbonImageSize)));

        public static readonly DependencyProperty IsInQuickAccessToolBarProperty =
            DependencyProperty.Register(nameof(IsInQuickAccessToolBar),
                                        typeof(bool),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsLabelVisibleProperty =
            DependencyProperty.Register(nameof(IsLabelVisible),
                                        typeof(bool),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty LabelGeometryProperty =
            DependencyProperty.Register(nameof(LabelGeometry),
                                        typeof(Geometry),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label),
                                        typeof(string),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty LabelTextAlignmentProperty =
            DependencyProperty.Register(nameof(LabelTextAlignment),
                                        typeof(TextAlignment),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(TextAlignment)));

        public static readonly DependencyProperty LargeImageSourceProperty =
            DependencyProperty.Register(nameof(LargeImageSource),
                                        typeof(ImageSource),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty SmallImageSourceProperty =
            DependencyProperty.Register(nameof(SmallImageSource),
                                        typeof(ImageSource),
                                        typeof(RibbonButtonContent),
                                        new FrameworkPropertyMetadata(default(ImageSource)));

        static RibbonButtonContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonButtonContent), new FrameworkPropertyMetadata(typeof(RibbonButtonContent)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonButtonContent), Resource.ControlStyle);
        }

        public bool HasTwoLines
        {
            get { return (bool)GetValue(HasTwoLinesProperty); }
            set { SetValue(HasTwoLinesProperty, value); }
        }

        public RibbonImageSize ImageSize
        {
            get { return (RibbonImageSize)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }

        public bool IsInQuickAccessToolBar
        {
            get { return (bool)GetValue(IsInQuickAccessToolBarProperty); }
            set { SetValue(IsInQuickAccessToolBarProperty, value); }
        }

        public bool IsLabelVisible
        {
            get { return (bool)GetValue(IsLabelVisibleProperty); }
            set { SetValue(IsLabelVisibleProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public Geometry LabelGeometry
        {
            get { return (Geometry)GetValue(LabelGeometryProperty); }
            set { SetValue(LabelGeometryProperty, value); }
        }

        public TextAlignment LabelTextAlignment
        {
            get { return (TextAlignment)GetValue(LabelTextAlignmentProperty); }
            set { SetValue(LabelTextAlignmentProperty, value); }
        }

        public ImageSource LargeImageSource
        {
            get { return (ImageSource)GetValue(LargeImageSourceProperty); }
            set { SetValue(LargeImageSourceProperty, value); }
        }

        public ImageSource SmallImageSource
        {
            get { return (ImageSource)GetValue(SmallImageSourceProperty); }
            set { SetValue(SmallImageSourceProperty, value); }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonButtonContent.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonButtonContent style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonButtonContent control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
