using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonSeparator : System.Windows.Controls.Ribbon.RibbonSeparator
    {
        #region Constructors

        static RibbonSeparator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonSeparator), new FrameworkPropertyMetadata(typeof(RibbonSeparator)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonSeparator), Resource.ControlStyle);
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonSeparator.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonSeparator style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonSeparator control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}