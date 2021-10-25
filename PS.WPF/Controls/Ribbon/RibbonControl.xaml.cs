using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonControl : System.Windows.Controls.Ribbon.RibbonControl
    {
        #region Constructors

        static RibbonControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonControl), new FrameworkPropertyMetadata(typeof(RibbonControl)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonControl), Resource.ControlStyle);
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonControl.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonControl style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonControl control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}