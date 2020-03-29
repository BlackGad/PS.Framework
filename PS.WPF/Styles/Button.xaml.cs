using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Styles
{
    public static class Button
    {
        #region Constants

        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Styles/Button.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ButtonStyle =
            ResourceDescriptor.Create<Style>(
                description: "Button style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor ButtonTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Button template",
                resourceDictionary: DictionaryLocation
            );

        #endregion
    }
}