using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Standard
{
    public static class Menu
    {
        #region Constants

        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Controls/Standard/Menu.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor ContextMenuStyle =
            ResourceDescriptor.Create<Style>(
                description: "ContextMenu style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor ContextMenuTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "ContextMenu template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor MenuItemStyle =
            ResourceDescriptor.Create<Style>(
                description: "Menu item style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor MenuScrollViewerStyle =
            ResourceDescriptor.Create<Style>(
                description: "Menu ScrollViewer style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor MenuScrollViewerTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Menu ScrollViewer template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor MenuStyle =
            ResourceDescriptor.Create<Style>(
                description: "Menu style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor MenuTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Menu template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor SeparatorStyle =
            ResourceDescriptor.Create<Style>(
                description: "Separator style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor SeparatorTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Separator template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor SubmenuTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Submenu template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor TopLevelHeaderTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Top level header template",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor TopLevelItemTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "Top level item template template",
                resourceDictionary: DictionaryLocation
            );

        #endregion
    }
}