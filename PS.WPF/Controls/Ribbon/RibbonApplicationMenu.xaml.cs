using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using PS.WPF.Extensions;
using PS.WPF.Resources;
using RibbonButton = PS.WPF.Controls.Ribbon.Controls.RibbonButton;
using RibbonMenuButton = PS.WPF.Controls.Ribbon.Controls.RibbonMenuButton;
using RibbonMenuItem = PS.WPF.Controls.Ribbon.Controls.RibbonMenuItem;
using RibbonSplitButton = PS.WPF.Controls.Ribbon.Controls.RibbonSplitButton;
using RibbonToggleButton = PS.WPF.Controls.Ribbon.Controls.RibbonToggleButton;

namespace PS.WPF.Controls.Ribbon
{
    public class RibbonApplicationMenu : System.Windows.Controls.Ribbon.RibbonApplicationMenu
    {
        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent),
                                        typeof(Brush),
                                        typeof(RibbonApplicationMenu),
                                        new FrameworkPropertyMetadata(default(Brush)));

        private object _currentItem;

        static RibbonApplicationMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonApplicationMenu), new FrameworkPropertyMetadata(typeof(RibbonApplicationMenu)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonApplicationMenu), Resource.ControlStyle);
        }

        public RibbonApplicationMenu()
        {
            System.Windows.Controls.Ribbon.RibbonQuickAccessToolBar.AddCloneHandler(this, OnRibbonQuickAccessToolBarClone);
        }

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is RibbonApplicationMenuItem ||
                item is RibbonApplicationSplitMenuItem ||
                item is RibbonSeparator ||
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
                if (dataTemplate == null) return new Controls.RibbonApplicationMenuItem();

                object templateContent = dataTemplate.LoadContent();
                if (templateContent is RibbonApplicationMenuItem ||
                    templateContent is RibbonApplicationSplitMenuItem ||
                    templateContent is RibbonSeparator ||
                    templateContent is RibbonGallery)
                {
                    return (DependencyObject)templateContent;
                }

                throw new InvalidOperationException("InvalidApplicationMenuOrItemContainer");
            }

            return new Controls.RibbonApplicationMenuItem();
        }

        private void OnRibbonQuickAccessToolBarClone(object sender, RibbonQuickAccessToolBarCloneEventArgs e)
        {
            if (!(e.InstanceToBeCloned is RibbonMenuItem menuItem)) return;

            if (menuItem.Items.Count == 0)
            {
                if (menuItem.IsCheckable)
                {
                    e.CloneInstance = new RibbonToggleButton();
                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, MenuItem.IsCheckedProperty, ToggleButton.IsCheckedProperty);
                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                            System.Windows.Controls.Ribbon.RibbonMenuItem.QuickAccessToolBarImageSourceProperty,
                                                            System.Windows.Controls.Ribbon.RibbonToggleButton.SmallImageSourceProperty);
                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                            System.Windows.Controls.Ribbon.RibbonMenuItem.QuickAccessToolBarIdProperty,
                                                            System.Windows.Controls.Ribbon.RibbonToggleButton.QuickAccessToolBarIdProperty);
                }
                else
                {
                    e.CloneInstance = new RibbonButton();
                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                            System.Windows.Controls.Ribbon.RibbonMenuItem.QuickAccessToolBarImageSourceProperty,
                                                            System.Windows.Controls.Ribbon.RibbonButton.SmallImageSourceProperty);
                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                            System.Windows.Controls.Ribbon.RibbonMenuItem.QuickAccessToolBarIdProperty,
                                                            System.Windows.Controls.Ribbon.RibbonButton.QuickAccessToolBarIdProperty);
                }

                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, MenuItem.CommandProperty, ButtonBase.CommandProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, MenuItem.CommandParameterProperty, ButtonBase.CommandParameterProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, MenuItem.CommandTargetProperty, ButtonBase.CommandTargetProperty);
            }
            else
            {
                if (menuItem.IsCheckable)
                {
                    e.CloneInstance = new RibbonSplitButton();

                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                            MenuItem.CommandProperty,
                                                            System.Windows.Controls.Ribbon.RibbonSplitButton.CommandProperty);
                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                            MenuItem.CommandParameterProperty,
                                                            System.Windows.Controls.Ribbon.RibbonSplitButton.CommandParameterProperty);
                    e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                            MenuItem.CommandTargetProperty,
                                                            System.Windows.Controls.Ribbon.RibbonSplitButton.CommandTargetProperty);
                }
                else
                {
                    e.CloneInstance = new RibbonMenuButton();
                }

                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                        System.Windows.Controls.Ribbon.RibbonMenuItem.QuickAccessToolBarImageSourceProperty,
                                                        SmallImageSourceProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance,
                                                        System.Windows.Controls.Ribbon.RibbonMenuItem.QuickAccessToolBarIdProperty,
                                                        QuickAccessToolBarIdProperty);

                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemBindingGroupProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemContainerStyleProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemContainerStyleSelectorProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemsPanelProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemsSourceProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemStringFormatProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemTemplateProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemTemplateSelectorProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, DisplayMemberPathProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, AlternationCountProperty);

                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, UsesItemContainerTemplateProperty);
                e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, ItemContainerTemplateSelectorProperty);
            }

            if (e.CloneInstance == null) return;

            e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, DataContextProperty);
            e.InstanceToBeCloned.TransferPropertyTo(e.CloneInstance, BindingGroupProperty);

            e.Handled = true;
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/RibbonApplicationMenu.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonApplicationMenu style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonApplicationMenu control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor MainButtonStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonApplicationMenu head button style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor MainButtonTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonApplicationMenu head button template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
