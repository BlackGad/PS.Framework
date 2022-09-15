using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonSplitButton : System.Windows.Controls.Ribbon.RibbonSplitButton
    {
        static RibbonSplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonSplitButton), new FrameworkPropertyMetadata(typeof(RibbonSplitButton)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonSplitButton), Resource.ControlStyle);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_MenuButton") is ItemsControl itemsControl)
            {
                itemsControl.ItemsSource = Items;
            }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonSplitButton.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonSplitButton style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonSplitButton control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
