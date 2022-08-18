using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonApplicationSplitMenuItem : System.Windows.Controls.Ribbon.RibbonApplicationSplitMenuItem
    {
        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent),
                                        typeof(Brush),
                                        typeof(RibbonApplicationSplitMenuItem),
                                        new FrameworkPropertyMetadata(default(Brush)));

        private static object CoerceIsChecked(DependencyObject sender, object value, CoerceValueCallback originalCallback)
        {
            if (sender is MenuItem menuItem &&
                menuItem.Command is ICommand command &&
                !command.CanExecute(menuItem.CommandParameter))
            {
                return menuItem.IsChecked;
            }

            return value;
        }

        private object _currentItem;

        static RibbonApplicationSplitMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonApplicationSplitMenuItem), new FrameworkPropertyMetadata(typeof(RibbonApplicationSplitMenuItem)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonApplicationSplitMenuItem), Resource.ControlStyle);
            IsCheckedProperty.Override(typeof(RibbonApplicationSplitMenuItem), coerce: CoerceIsChecked);
        }

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

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

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonApplicationSplitMenuItem.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonApplicationSplitMenuItem style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonApplicationSplitMenuItem control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
