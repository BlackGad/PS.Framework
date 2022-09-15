using System;
using System.Windows;
using PS.WPF.Resources;

namespace PS.Shell.Module.Ribbon
{
    public static class XamlResources
    {
        private static readonly Uri Default = new Uri("/PS.Shell.Module.Ribbon;component/XamlResources.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor RibbonStyle = ResourceDescriptor.Create<Style>(Default);
    }
}
