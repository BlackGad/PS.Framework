using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonApplicationMenuItem : System.Windows.Controls.Ribbon.RibbonApplicationMenuItem
    {
        #region Property definitions

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent),
                                        typeof(Brush),
                                        typeof(RibbonApplicationMenuItem),
                                        new FrameworkPropertyMetadata(default(Brush)));

        #endregion

        private object _currentItem;

        #region Constructors

        static RibbonApplicationMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonApplicationMenuItem), new FrameworkPropertyMetadata(typeof(RibbonApplicationMenuItem)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonApplicationMenuItem), Resource.ControlStyle);
        }

        #endregion

        #region Properties

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        #endregion

        #region Override members

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is System.Windows.Controls.Ribbon.RibbonApplicationMenuItem ||
                item is System.Windows.Controls.Ribbon.RibbonApplicationSplitMenuItem ||
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
                if (dataTemplate == null) return new RibbonApplicationMenuItem();

                object templateContent = dataTemplate.LoadContent();
                if (templateContent is System.Windows.Controls.Ribbon.RibbonApplicationMenuItem ||
                    templateContent is System.Windows.Controls.Ribbon.RibbonApplicationSplitMenuItem ||
                    templateContent is System.Windows.Controls.Ribbon.RibbonSeparator ||
                    templateContent is RibbonGallery)
                {
                    return (DependencyObject)templateContent;
                }

                throw new InvalidOperationException("InvalidApplicationMenuOrItemContainer");
            }

            return new RibbonApplicationMenuItem();
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonApplicationMenuItem.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonApplicationMenuItem style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonApplicationMenuItem control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}