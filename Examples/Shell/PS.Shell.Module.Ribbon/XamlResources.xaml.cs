using System;
using System.Windows;
using System.Windows.Media;
using PS.WPF.Resources;

namespace PS.Shell.Module.Ribbon
{
    public static class XamlResources
    {
        #region Constants

        private static readonly Uri Default =
            new Uri("/PS.Shell.Module.Ribbon;component/XamlResources.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor RibbonStyle = ResourceDescriptor.Create<Style>(Default);
        public static readonly ResourceDescriptor ContextualBackgroundBrush = ResourceDescriptor.Create<SolidColorBrush>(Default);
        public static readonly ResourceDescriptor RibbonAccentBrush = ResourceDescriptor.Create<SolidColorBrush>(Default);
        public static readonly ResourceDescriptor RibbonBackgroundBrush = ResourceDescriptor.Create<SolidColorBrush>(Default);
        public static readonly ResourceDescriptor RibbonBorderBrushBrush = ResourceDescriptor.Create<SolidColorBrush>(Default);
        public static readonly ResourceDescriptor RibbonForegroundBrush = ResourceDescriptor.Create<SolidColorBrush>(Default);
        public static readonly ResourceDescriptor RibbonHeaderBackgroundBrush = ResourceDescriptor.Create<SolidColorBrush>(Default);

        #endregion
    }
}