using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class Button : System.Windows.Controls.Button
    {
        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
            ResourceHelper.SetDefaultStyle(typeof(Button), Resource.ControlStyle);
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/Button.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);
        }

        #endregion
    }
}
