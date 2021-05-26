using System;
using System.Windows;
using PS.WPF.Resources;

namespace PS.Shell.Infrastructure
{
    public static class XamlResources
    {
        #region Constants

        private static readonly Uri Default =
            new Uri("/PS.Shell.Infrastructure;component/XamlResources.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ConfirmationStyle =
            ResourceDescriptor.Create<Style>(description: "Default window style for confirmation view",
                                             resourceDictionary: Default);

        public static readonly ResourceDescriptor NotificationStyle =
            ResourceDescriptor.Create<Style>(description: "Default window style for notification view",
                                             resourceDictionary: Default);

        #endregion
    }
}