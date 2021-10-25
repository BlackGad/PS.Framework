using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public static class RibbonHorizontalScrollViewer
    {
        #region Constants

        private static readonly Uri Default =
            new Uri("/PS.WPF;component/Controls/Ribbon/RibbonHorizontalScrollViewer.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ControlStyle =
            ResourceDescriptor.Create<Style>(description: "Default RibbonHorizontalScrollViewer style",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor ControlTemplate =
            ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonHorizontalScrollViewer control template",
                                                       resourceDictionary: Default);

        public static readonly ResourceDescriptor RepeatButtonStyle =
            ResourceDescriptor.Create<Style>(description: "Default RibbonHorizontalScrollViewer RepeatButton style",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor RepeatButtonTemplate =
            ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonHorizontalScrollViewer RepeatButton template",
                                                       resourceDictionary: Default);

        #endregion
    }
}