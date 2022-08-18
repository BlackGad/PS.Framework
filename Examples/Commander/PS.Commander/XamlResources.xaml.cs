using System;
using System.Windows;
using PS.WPF.Resources;

namespace PS.Commander
{
    public static class XamlResources
    {
        private static readonly Uri Default =
            new Uri("/PS.Commander;component/XamlResources.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor NotificationStyle =
            ResourceDescriptor.Create<Style>(description: "Default window style for notification view",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor ShellWindowStyle =
            ResourceDescriptor.Create<Style>(description: "Default style for window",
                                             resourceDictionary: Default);
    }
}
