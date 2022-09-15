using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonMenuButton : System.Windows.Controls.Ribbon.RibbonMenuButton
    {
        private object _currentItem;

        static RibbonMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonMenuButton), new FrameworkPropertyMetadata(typeof(RibbonMenuButton)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonMenuButton), Resource.ControlStyle);
        }

        protected override void OnDismissPopup(RibbonDismissPopupEventArgs e)
        {
            if (this.HasVisualParent(e.OriginalSource as DependencyObject))
            {
                e.Handled = true;
                return;
            }

            base.OnDismissPopup(e);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is System.Windows.Controls.Ribbon.RibbonMenuItem ||
                item is System.Windows.Controls.Ribbon.RibbonSeparator ||
                item is RibbonGallery)
            {
                return true;
            }

            _currentItem = item;
            return false;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (ItemContainerStyleSelector != null && element is FrameworkElement container)
            {
                var style = ItemContainerStyleSelector.SelectStyle(item, element);
                if (style != null) container.Style = style;
            }

            if (ItemContainerTemplateSelector != null && element is MenuItem menuItem)
            {
                menuItem.SetValueIfDefault(ItemContainerTemplateSelectorProperty, ItemContainerTemplateSelector);
                menuItem.SetValueIfDefault(UsesItemContainerTemplateProperty, UsesItemContainerTemplate);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var currentItem = _currentItem;
            _currentItem = null;

            if (UsesItemContainerTemplate)
            {
                var dataTemplate = ItemContainerTemplateSelector.SelectTemplate(currentItem, this);
                if (dataTemplate == null) return new RibbonMenuItem();

                object templateContent = dataTemplate.LoadContent();
                if (templateContent is System.Windows.Controls.Ribbon.RibbonMenuItem ||
                    templateContent is System.Windows.Controls.Ribbon.RibbonSeparator ||
                    templateContent is RibbonGallery)
                {
                    return (DependencyObject)templateContent;
                }

                throw new InvalidOperationException("InvalidApplicationMenuOrItemContainer");
            }

            return new RibbonMenuItem();
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonMenuButton.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonMenuButton style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonMenuButton control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
