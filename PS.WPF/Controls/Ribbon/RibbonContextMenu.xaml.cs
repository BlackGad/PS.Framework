using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public static class RibbonContextMenu
    {
        #region Constants

        private static readonly Uri Default =
            new Uri("/PS.WPF;component/Controls/Ribbon/RibbonContextMenu.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ControlStyle =
            ResourceDescriptor.Create<Style>(description: "Default RibbonContextMenu style",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor ControlTemplate =
            ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonContextMenu control template",
                                                       resourceDictionary: Default);

        #endregion
    }
}