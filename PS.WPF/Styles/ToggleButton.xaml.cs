using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Styles
{
    public static class ToggleButton
    {
        #region Constants

        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Styles/ToggleButton.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ToggleButtonStyle =
            ResourceDescriptor.Create<Style>(
                description: "ToggleButton style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor ToggleButtonTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "ToggleButton template",
                resourceDictionary: DictionaryLocation
            );

        #endregion
    }
}