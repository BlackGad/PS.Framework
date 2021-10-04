using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using PS.Patterns.Aware;
using PS.WPF.Extensions;
using PS.WPF.Patterns.Command;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonMenuItem : System.Windows.Controls.Ribbon.RibbonMenuItem
    {
        #region Property definitions

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent),
                                        typeof(Brush),
                                        typeof(RibbonMenuItem),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty OrderProperty =
            DependencyProperty.Register(nameof(Order),
                                        typeof(int),
                                        typeof(RibbonMenuItem),
                                        new FrameworkPropertyMetadata(default(int)));

        #endregion

        private object _currentItem;

        #region Constructors

        static RibbonMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonMenuItem), new FrameworkPropertyMetadata(typeof(RibbonMenuItem)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonMenuItem), Resource.ControlStyle);
            IsEnabledProperty.Override(typeof(RibbonMenuItem), coerce: (sender, value, original) => true);
        }

        public RibbonMenuItem()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Properties

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public int Order
        {
            get { return (int)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        #endregion

        #region Override members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Action") is Button button)
            {
                button.Click += (sender, args) => { RaiseEvent(new RibbonDismissPopupEventArgs()); };
            }
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

                if (container is ItemsControl itemsControl)
                {
                    itemsControl.SetValueIfDefault(ItemContainerStyleSelectorProperty, ItemContainerStyleSelector);
                }
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

            if (currentItem is IIsSeparatorAware isSeparatorAware && isSeparatorAware.IsSeparator)
            {
                return new RibbonSeparator();
            }

            return new RibbonMenuItem();
        }

        #endregion

        #region Event handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Command is IUICommand command)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonMenuItem.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonMenuItem style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonMenuItem control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}