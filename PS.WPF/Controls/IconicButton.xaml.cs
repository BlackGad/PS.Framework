using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class IconicButton : System.Windows.Controls.Button
    {
        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register("Geometry",
                                        typeof(Geometry),
                                        typeof(IconicButton),
                                        new FrameworkPropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight",
                                        typeof(double?),
                                        typeof(IconicButton),
                                        new FrameworkPropertyMetadata(default(double?)));

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth",
                                        typeof(double?),
                                        typeof(IconicButton),
                                        new FrameworkPropertyMetadata(default(double?)));

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register("Spacing",
                                        typeof(double),
                                        typeof(IconicButton),
                                        new FrameworkPropertyMetadata(default(double)));

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch",
                                        typeof(Stretch),
                                        typeof(IconicButton),
                                        new FrameworkPropertyMetadata(default(Stretch)));

        static IconicButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconicButton), new FrameworkPropertyMetadata(typeof(IconicButton)));
            ResourceHelper.SetDefaultStyle(typeof(IconicButton), Resource.ControlStyle);
        }

        public Geometry Geometry
        {
            get { return (Geometry)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        public double? IconHeight
        {
            get { return (double?)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public double? IconWidth
        {
            get { return (double?)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/IconicButton.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default IconicButton style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default IconicButton control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
