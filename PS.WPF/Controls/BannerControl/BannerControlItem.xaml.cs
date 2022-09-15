using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.BannerControl
{
    public class BannerControlItem : ContentControl
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius),
                                        typeof(CornerRadius),
                                        typeof(BannerControlItem),
                                        new FrameworkPropertyMetadata(default(CornerRadius)));

        static BannerControlItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BannerControlItem), new FrameworkPropertyMetadata(typeof(BannerControlItem)));
            ResourceHelper.SetDefaultStyle(typeof(BannerControlItem), Resource.ControlStyle);
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/BannerControl/BannerControlItem.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);
        }

        #endregion
    }
}
