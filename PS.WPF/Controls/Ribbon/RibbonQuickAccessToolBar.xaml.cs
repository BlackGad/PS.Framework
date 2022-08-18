using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonQuickAccessToolBar : System.Windows.Controls.Ribbon.RibbonQuickAccessToolBar
    {
        static RibbonQuickAccessToolBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonQuickAccessToolBar), new FrameworkPropertyMetadata(typeof(RibbonQuickAccessToolBar)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonQuickAccessToolBar), Resource.ControlStyle);
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonQuickAccessToolBar.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonQuickAccessToolBar style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonQuickAccessToolBar control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
