using System;
using System.Windows;
using PS.WPF.Resources;

namespace PS.Shell
{
    public static class XamlResources
    {
        private static readonly Uri Default = new Uri("/PS.Shell;component/XamlResources.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ShellWindowStyle =
            ResourceDescriptor.Create<Style>(description: "Default style for window",
                                             resourceDictionary: Default);
    }
}
