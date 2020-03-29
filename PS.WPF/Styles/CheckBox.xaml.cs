using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Styles
{
    public static class CheckBox
    {
        #region Constants

        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Styles/CheckBox.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor CheckBoxStyle =
            ResourceDescriptor.Create<Style>(
                description: "CheckBox style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor CheckBoxTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "CheckBox template",
                resourceDictionary: DictionaryLocation
            );

        #endregion
    }
}