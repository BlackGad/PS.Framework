using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon
{
    public class Ribbon : System.Windows.Controls.Ribbon.Ribbon
    {
        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent),
                                        typeof(Brush),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CaptionFontFamilyProperty =
            DependencyProperty.Register(nameof(CaptionFontFamily),
                                        typeof(FontFamily),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(FontFamily)));

        public static readonly DependencyProperty CaptionFontSizeProperty =
            DependencyProperty.Register(nameof(CaptionFontSize),
                                        typeof(double?),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(double?)));

        public static readonly DependencyProperty CaptionForegroundProperty =
            DependencyProperty.Register(nameof(CaptionForeground),
                                        typeof(Brush),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ContextualTabGroupHeaderTemplateSelectorProperty =
            DependencyProperty.Register(nameof(ContextualTabGroupHeaderTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty ContextualTabGroupStyleSelectorProperty =
            DependencyProperty.Register(nameof(ContextualTabGroupStyleSelector),
                                        typeof(System.Windows.Controls.StyleSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.StyleSelector)));

        public static readonly DependencyProperty ContextualTabHeaderBackgroundProperty =
            DependencyProperty.Register(nameof(ContextualTabHeaderBackground),
                                        typeof(Brush),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ControlDataTemplateSelectorProperty =
            DependencyProperty.Register(nameof(ControlDataTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty ControlStyleSelectorProperty =
            DependencyProperty.Register(nameof(ControlStyleSelector),
                                        typeof(System.Windows.Controls.StyleSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.StyleSelector)));

        public static readonly DependencyProperty SelectedTabHeaderBackgroundProperty =
            DependencyProperty.Register(nameof(SelectedTabHeaderBackground),
                                        typeof(Brush),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty SelectedTabHeaderForegroundProperty =
            DependencyProperty.Register(nameof(SelectedTabHeaderForeground),
                                        typeof(Brush),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty SuppressQuickAccessItemsProperty =
            DependencyProperty.Register(nameof(SuppressQuickAccessItems),
                                        typeof(bool),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty TabGroupDataTemplateSelectorProperty =
            DependencyProperty.Register(nameof(TabGroupDataTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty TabGroupStyleSelectorProperty =
            DependencyProperty.Register(nameof(TabGroupStyleSelector),
                                        typeof(System.Windows.Controls.StyleSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.StyleSelector)));

        public static readonly DependencyProperty TabHeaderBackgroundProperty =
            DependencyProperty.Register(nameof(TabHeaderBackground),
                                        typeof(Brush),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty TabHeaderForegroundProperty =
            DependencyProperty.Register(nameof(TabHeaderForeground),
                                        typeof(Brush),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty TabHeaderStyleSelectorProperty =
            DependencyProperty.Register(nameof(TabHeaderStyleSelector),
                                        typeof(System.Windows.Controls.StyleSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.StyleSelector)));

        public static readonly DependencyProperty TabHeaderTemplateSelectorProperty =
            DependencyProperty.Register(nameof(TabHeaderTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty TitleTemplateSelectorProperty =
            DependencyProperty.Register(nameof(TitleTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(Ribbon),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        private static object OnCoerceContextMenu(DependencyObject sender, object value, CoerceValueCallback originCallback)
        {
            var menu = originCallback?.Invoke(sender, value) ?? value;
            if (menu is ContextMenu contextMenu)
            {
                var ribbon = (Ribbon)sender;
                ribbon?.ValidateContextMenu(contextMenu);
            }

            return menu;
        }

        static Ribbon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ribbon), new FrameworkPropertyMetadata(typeof(Ribbon)));
            ResourceHelper.SetDefaultStyle(typeof(Ribbon), Resource.ControlStyle);
            QuickAccessToolBarProperty.Override(typeof(Ribbon), coerce: (sender, baseValue, original) => baseValue ?? new RibbonQuickAccessToolBar());
            ContextMenuProperty.Override(typeof(Ribbon), coerce: OnCoerceContextMenu);
        }

        public Ribbon()
        {
            CoerceValue(QuickAccessToolBarProperty);
        }

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public FontFamily CaptionFontFamily
        {
            get { return (FontFamily)GetValue(CaptionFontFamilyProperty); }
            set { SetValue(CaptionFontFamilyProperty, value); }
        }

        public double? CaptionFontSize
        {
            get { return (double?)GetValue(CaptionFontSizeProperty); }
            set { SetValue(CaptionFontSizeProperty, value); }
        }

        public Brush CaptionForeground
        {
            get { return (Brush)GetValue(CaptionForegroundProperty); }
            set { SetValue(CaptionForegroundProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector ContextualTabGroupHeaderTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(ContextualTabGroupHeaderTemplateSelectorProperty); }
            set { SetValue(ContextualTabGroupHeaderTemplateSelectorProperty, value); }
        }

        public System.Windows.Controls.StyleSelector ContextualTabGroupStyleSelector
        {
            get { return (System.Windows.Controls.StyleSelector)GetValue(ContextualTabGroupStyleSelectorProperty); }
            set { SetValue(ContextualTabGroupStyleSelectorProperty, value); }
        }

        public Brush ContextualTabHeaderBackground
        {
            get { return (Brush)GetValue(ContextualTabHeaderBackgroundProperty); }
            set { SetValue(ContextualTabHeaderBackgroundProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector ControlDataTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(ControlDataTemplateSelectorProperty); }
            set { SetValue(ControlDataTemplateSelectorProperty, value); }
        }

        public System.Windows.Controls.StyleSelector ControlStyleSelector
        {
            get { return (System.Windows.Controls.StyleSelector)GetValue(ControlStyleSelectorProperty); }
            set { SetValue(ControlStyleSelectorProperty, value); }
        }

        public Brush SelectedTabHeaderBackground
        {
            get { return (Brush)GetValue(SelectedTabHeaderBackgroundProperty); }
            set { SetValue(SelectedTabHeaderBackgroundProperty, value); }
        }

        public Brush SelectedTabHeaderForeground
        {
            get { return (Brush)GetValue(SelectedTabHeaderForegroundProperty); }
            set { SetValue(SelectedTabHeaderForegroundProperty, value); }
        }

        public bool SuppressQuickAccessItems
        {
            get { return (bool)GetValue(SuppressQuickAccessItemsProperty); }
            set { SetValue(SuppressQuickAccessItemsProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector TabGroupDataTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(TabGroupDataTemplateSelectorProperty); }
            set { SetValue(TabGroupDataTemplateSelectorProperty, value); }
        }

        public System.Windows.Controls.StyleSelector TabGroupStyleSelector
        {
            get { return (System.Windows.Controls.StyleSelector)GetValue(TabGroupStyleSelectorProperty); }
            set { SetValue(TabGroupStyleSelectorProperty, value); }
        }

        public Brush TabHeaderBackground
        {
            get { return (Brush)GetValue(TabHeaderBackgroundProperty); }
            set { SetValue(TabHeaderBackgroundProperty, value); }
        }

        public Brush TabHeaderForeground
        {
            get { return (Brush)GetValue(TabHeaderForegroundProperty); }
            set { SetValue(TabHeaderForegroundProperty, value); }
        }

        public System.Windows.Controls.StyleSelector TabHeaderStyleSelector
        {
            get { return (System.Windows.Controls.StyleSelector)GetValue(TabHeaderStyleSelectorProperty); }
            set { SetValue(TabHeaderStyleSelectorProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector TabHeaderTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(TabHeaderTemplateSelectorProperty); }
            set { SetValue(TabHeaderTemplateSelectorProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector TitleTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(TitleTemplateSelectorProperty); }
            set { SetValue(TitleTemplateSelectorProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Reset TabHeaderItemsControl.ItemsSource from default
            if (GetTemplateChild("TabHeaderItemsControl") is RibbonTabHeaderItemsControl tabHeaderItemsControl)
            {
                var binding = new PriorityBinding
                {
                    Bindings =
                    {
                        new Binding
                        {
                            Source = tabHeaderItemsControl.ItemsSource
                        },
                        new Binding
                        {
                            Source = this,
                            Path = new PropertyPath(ItemsSourceProperty)
                        }
                    }
                };
                tabHeaderItemsControl.SetBinding(ItemsSourceProperty, binding);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RibbonTab();
        }

        public void ValidateContextMenu(ContextMenu contextMenu)
        {
            if (!SuppressQuickAccessItems) return;

            var itemsToRemove = new List<object>();

            foreach (var item in contextMenu.Items)
            {
                if (item is MenuItem menuItem && !menuItem.Header.GetEffectiveString().Contains("Quick"))
                {
                    break;
                }

                itemsToRemove.Add(item);
            }

            itemsToRemove.ForEach(i => contextMenu.Items.Remove(i));
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Ribbon.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default Ribbon style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default Ribbon control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
