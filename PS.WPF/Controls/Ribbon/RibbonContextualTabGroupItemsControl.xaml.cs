using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonContextualTabGroupItemsControl : System.Windows.Controls.Ribbon.RibbonContextualTabGroupItemsControl
    {
        static RibbonContextualTabGroupItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonContextualTabGroupItemsControl),
                                                     new FrameworkPropertyMetadata(typeof(RibbonContextualTabGroupItemsControl)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonContextualTabGroupItemsControl), Resource.ControlStyle);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RibbonContextualTabGroup();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Ribbon is Ribbon ribbonExtended)
            {
                this.SetBindingIfDefault(ItemTemplateSelectorProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.ContextualTabGroupHeaderTemplateSelectorProperty),
                                             Source = ribbonExtended
                                         });

                this.SetBindingIfDefault(ItemContainerStyleSelectorProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.ContextualTabGroupStyleSelectorProperty),
                                             Source = ribbonExtended
                                         });
            }

            if (Ribbon is System.Windows.Controls.Ribbon.Ribbon ribbon)
            {
                this.SetBindingIfDefault(ItemTemplateProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(System.Windows.Controls.Ribbon.Ribbon.ContextualTabGroupHeaderTemplateProperty),
                                             Source = ribbon
                                         });
                this.SetBindingIfDefault(ItemContainerStyleProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(System.Windows.Controls.Ribbon.Ribbon.ContextualTabGroupStyleProperty),
                                             Source = ribbon
                                         });
            }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonContextualTabGroupItemsControl.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonContextualTabGroupItemsControl style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonContextualTabGroupItemsControl control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
