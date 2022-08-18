using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Native
{
    public static class ComboBox
    {
        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Controls/Native/ComboBox.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ComboBoxItemStyle =
            ResourceDescriptor.Create<Style>(description: "ComboBoxItem style",
                                             resourceDictionary: DictionaryLocation);

        public static readonly ResourceDescriptor ComboBoxItemTemplate =
            ResourceDescriptor.Create<ControlTemplate>(description: "ComboBoxItem template",
                                                       resourceDictionary: DictionaryLocation);

        public static readonly ResourceDescriptor ComboBoxStyle =
            ResourceDescriptor.Create<Style>(description: "ComboBox style",
                                             resourceDictionary: DictionaryLocation);

        public static readonly ResourceDescriptor ComboBoxTemplate =
            ResourceDescriptor.Create<ControlTemplate>(description: "ComboBox template",
                                                       resourceDictionary: DictionaryLocation);
    }
}
