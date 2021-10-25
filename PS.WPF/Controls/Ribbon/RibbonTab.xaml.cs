using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonTab : System.Windows.Controls.Ribbon.RibbonTab
    {
        #region Constructors

        static RibbonTab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonTab), new FrameworkPropertyMetadata(typeof(RibbonTab)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonTab), Resource.ControlStyle);
        }

        #endregion

        #region Override members

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RibbonGroup();
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
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.TabGroupDataTemplateSelectorProperty)
                                         });

                this.SetBindingIfDefault(ItemContainerStyleSelectorProperty,
                                         new Binding
                                         {
                                             Source = extendedRibbon,
                                             Path = new PropertyPath(WPF.Controls.Ribbon.Ribbon.TabGroupStyleSelectorProperty)
                                         });
            }
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonTab.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonTab style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonTab control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}