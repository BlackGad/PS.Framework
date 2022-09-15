using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class SuggestListViewItem : ListViewItem
    {
        static SuggestListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuggestListViewItem), new FrameworkPropertyMetadata(typeof(SuggestListViewItem)));
            ResourceHelper.SetDefaultStyle(typeof(SuggestListViewItem), Resource.ControlStyle);
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/SuggestListView/SuggestListViewItem.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default SuggestListViewItem style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default SuggestListViewItem control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
