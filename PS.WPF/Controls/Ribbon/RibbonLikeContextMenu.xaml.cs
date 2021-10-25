using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Media;
using PS.Patterns.Aware;
using PS.WPF.Extensions;
using PS.WPF.Resources;
using RibbonMenuItem = PS.WPF.Controls.Ribbon.Controls.RibbonMenuItem;
using RibbonSeparator = PS.WPF.Controls.Ribbon.Controls.RibbonSeparator;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonLikeContextMenu : ContextMenu
    {
        #region Property definitions

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent),
                                        typeof(Brush),
                                        typeof(RibbonLikeContextMenu),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ItemsCommandParameterProperty =
            DependencyProperty.Register(nameof(ItemsCommandParameter),
                                        typeof(object),
                                        typeof(RibbonLikeContextMenu),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register(nameof(MouseOverBackground),
                                        typeof(Brush),
                                        typeof(RibbonLikeContextMenu),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.Register(nameof(MouseOverBorderBrush),
                                        typeof(Brush),
                                        typeof(RibbonLikeContextMenu),
                                        new FrameworkPropertyMetadata(default(Brush)));

        /// <summary>
        ///     DependencyProperty for Ribbon property.
        /// </summary>
        public static readonly DependencyProperty RibbonProperty =
            RibbonControlService.RibbonProperty.AddOwner(typeof(RibbonLikeContextMenu));

        #endregion

        private object _currentItem;

        #region Constructors

        static RibbonLikeContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonLikeContextMenu), new FrameworkPropertyMetadata(typeof(RibbonLikeContextMenu)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonLikeContextMenu), Resource.ControlStyle);
        }

        public RibbonLikeContextMenu()
        {
            AddHandler(RibbonControlService.DismissPopupEvent, new RibbonDismissPopupEventHandler(OnDismissPopup));
        }

        #endregion

        #region Properties

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public object ItemsCommandParameter
        {
            get { return GetValue(ItemsCommandParameterProperty); }
            set { SetValue(ItemsCommandParameterProperty, value); }
        }

        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }

        /// <summary>
        ///     This property is used to access Ribbon
        /// </summary>
        public System.Windows.Controls.Ribbon.Ribbon Ribbon
        {
            get { return RibbonControlService.GetRibbon(this); }
        }

        #endregion

        #region Override members

        protected override DependencyObject GetContainerForItemOverride()
        {
            var currentItem = _currentItem;
            _currentItem = null;

            if (UsesItemContainerTemplate)
            {
                var dataTemplate = ItemContainerTemplateSelector.SelectTemplate(currentItem, this);
                if (dataTemplate == null) return new RibbonMenuItem();

                object templateContent = dataTemplate.LoadContent();
                if (templateContent is MenuItem item)
                {
                    return item;
                }

                throw new InvalidOperationException("InvalidApplicationMenuOrItemContainer");
            }

            if (currentItem is IIsSeparatorAware isSeparatorAware && isSeparatorAware.IsSeparator)
            {
                return new RibbonSeparator();
            }

            return new RibbonMenuItem();
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

            var itemsCommandParameterPropertySource = DependencyPropertyHelper.GetValueSource(this, ItemsCommandParameterProperty);
            if (itemsCommandParameterPropertySource.BaseValueSource > BaseValueSource.Default)
            {
                element.SetBindingIfDefault(MenuItem.CommandParameterProperty,
                                            new Binding
                                            {
                                                Source = this,
                                                Path = new PropertyPath(ItemsCommandParameterProperty)
                                            });
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is MenuItem) return true;

            _currentItem = item;
            return false;
        }

        #endregion

        #region Event handlers

        private void OnDismissPopup(object sender, RibbonDismissPopupEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonLikeContextMenu.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonLikeContextMenu style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonLikeContextMenu control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}