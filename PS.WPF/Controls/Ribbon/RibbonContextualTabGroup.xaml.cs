using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonContextualTabGroup : System.Windows.Controls.Ribbon.RibbonContextualTabGroup
    {
        #region Constructors

        static RibbonContextualTabGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonContextualTabGroup), new FrameworkPropertyMetadata(typeof(RibbonContextualTabGroup)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonContextualTabGroup), Resource.ControlStyle);
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonContextualTabGroup.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonContextualTabGroup style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonContextualTabGroup control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}