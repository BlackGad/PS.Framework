using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonTabHeaderItemsControl : System.Windows.Controls.Ribbon.RibbonTabHeaderItemsControl
    {
        /// <summary>
        /// DependencyProperty for Ribbon property.
        /// </summary>
        public static readonly DependencyProperty RibbonProperty =
            RibbonControlService.RibbonProperty.AddOwner(typeof(RibbonTabHeader));

        static RibbonTabHeaderItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonTabHeaderItemsControl), new FrameworkPropertyMetadata(typeof(RibbonTabHeaderItemsControl)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonTabHeaderItemsControl), Resource.ControlStyle);
        }

        /// <summary>
        /// This property is used to access Ribbon
        /// </summary>
        public System.Windows.Controls.Ribbon.Ribbon Ribbon
        {
            get { return RibbonControlService.GetRibbon(this); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Ribbon is Ribbon ribbonExtended)
            {
                this.SetBindingIfDefault(ItemTemplateSelectorProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.TabHeaderTemplateSelectorProperty),
                                             Source = ribbonExtended
                                         });

                this.SetBindingIfDefault(ItemContainerStyleSelectorProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.TabHeaderStyleSelectorProperty),
                                             Source = ribbonExtended
                                         });
            }

            if (Ribbon is System.Windows.Controls.Ribbon.Ribbon ribbon)
            {
                this.SetBindingIfDefault(ItemTemplateProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(System.Windows.Controls.Ribbon.Ribbon.TabHeaderTemplateProperty),
                                             Source = ribbon
                                         });
                this.SetBindingIfDefault(ItemContainerStyleProperty,
                                         new Binding
                                         {
                                             Path = new PropertyPath(System.Windows.Controls.Ribbon.Ribbon.TabHeaderStyleProperty),
                                             Source = ribbon
                                         });
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RibbonTabHeader();
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonTabHeaderItemsControl.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonTabHeaderItemsControl style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonTabHeaderItemsControl control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
