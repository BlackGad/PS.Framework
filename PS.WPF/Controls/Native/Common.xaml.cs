using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Native
{
    public static class Common
    {
        public static readonly Uri DictionaryLocation;

        public static readonly ResourceDescriptor FlatDataGridCellStyle =
            ResourceDescriptor.Create<Style>(
                description: "Flat DataGrid cell style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor FlatDataGridColumnHeaderStyle =
            ResourceDescriptor.Create<Style>(
                description: "Flat DataGrid column header style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor FlatDataGridColumnHeaderTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Flat DataGrid column header template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor FlatDataGridStyle =
            ResourceDescriptor.Create<Style>(
                description: "Flat DataGrid style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor FocusRectangleStyle =
            ResourceDescriptor.Create<Style>(
                description: "Focus rectangle style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor Heading0TextBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Heading 0 TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor Heading1ExtraTextBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Heading extra TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor Heading1TextBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Heading 1 TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor NormalStrongTextBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Normal strong TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor NormalTextBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Normal TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor NormalTextBoxStyle =
            ResourceDescriptor.Create<Style>(
                description: "Normal TextBox style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor NormalTextBoxTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Normal TextBox template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor QuoteTextBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Quote TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor RadioButtonStyle =
            ResourceDescriptor.Create<Style>(
                description: "RadioButton style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor RadioButtonTemplate =
            ResourceDescriptor.Create<Style>(
                description: "RadioButton template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor TableColumnHeaderBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Table column header TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor TitleTextBlockStyle =
            ResourceDescriptor.Create<Style>(
                description: "Title TextBlock style",
                resourceDictionary: DictionaryLocation
            );

        static Common()
        {
            DictionaryLocation = new Uri("/PS.WPF;component/Controls/Native/Common.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
