using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonGroup : System.Windows.Controls.Ribbon.RibbonGroup
    {
        static RibbonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonGroup), new FrameworkPropertyMetadata(typeof(RibbonGroup)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonGroup), Resource.ControlStyle);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RibbonControl();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Ribbon is Ribbon extendedRibbon)
            {
                this.SetBindingIfDefault(ItemTemplateSelectorProperty,
                                         new Binding
                                         {
                                             Source = extendedRibbon,
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.ControlDataTemplateSelectorProperty)
                                         });

                this.SetBindingIfDefault(ItemContainerStyleSelectorProperty,
                                         new Binding
                                         {
                                             Source = extendedRibbon,
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.ControlStyleSelectorProperty)
                                         });
            }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonGroup.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor CollapsedControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonGroup collapsed control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonGroup style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonGroup control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
