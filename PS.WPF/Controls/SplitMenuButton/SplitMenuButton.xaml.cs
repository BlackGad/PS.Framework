using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.Navigation;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    [ContentProperty("Items")]
    public class SplitMenuButton : Control,
                                   ICommandSource
    {
        private static readonly DependencyPropertyKey HasHeaderPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(HasHeader),
                                                typeof(bool),
                                                typeof(SplitMenuButton),
                                                new FrameworkPropertyMetadata(default(bool)));

        private static readonly DependencyPropertyKey HasItemsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(HasItems),
                                                typeof(bool),
                                                typeof(SplitMenuButton),
                                                new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter),
                                        typeof(object),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command),
                                        typeof(ICommand),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register(nameof(CommandTarget),
                                        typeof(IInputElement),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(IInputElement)));

        public static readonly DependencyProperty HasHeaderProperty = HasHeaderPropertyKey.DependencyProperty;

        public static readonly DependencyProperty HasItemsProperty = HasItemsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header),
                                        typeof(object),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(OnHeaderChanged));

        public static readonly DependencyProperty HeaderStringFormatProperty =
            DependencyProperty.Register(nameof(HeaderStringFormat),
                                        typeof(string),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate),
                                        typeof(System.Windows.DataTemplate),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(System.Windows.DataTemplate)));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
            DependencyProperty.Register(nameof(HeaderTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty IsMenuOpenedProperty =
            DependencyProperty.Register(nameof(IsMenuOpened),
                                        typeof(bool),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource),
                                        typeof(IEnumerable),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty MenuItemContainerStyleProperty =
            DependencyProperty.Register(nameof(MenuItemContainerStyle),
                                        typeof(Style),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(Style)));

        public static readonly DependencyProperty MenuItemContainerStyleSelectorProperty =
            DependencyProperty.Register(nameof(MenuItemContainerStyleSelector),
                                        typeof(System.Windows.Controls.StyleSelector),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.StyleSelector)));

        public static readonly DependencyProperty MenuItemsPanelProperty =
            DependencyProperty.Register(nameof(MenuItemsPanel),
                                        typeof(ItemsPanelTemplate),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(ItemsPanelTemplate)));

        public static readonly DependencyProperty MenuItemTemplateProperty =
            DependencyProperty.Register(nameof(MenuItemTemplate),
                                        typeof(System.Windows.DataTemplate),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(System.Windows.DataTemplate)));

        public static readonly DependencyProperty MenuItemTemplateSelectorProperty =
            DependencyProperty.Register(nameof(MenuItemTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty MenuStyleProperty =
            DependencyProperty.Register(nameof(MenuStyle),
                                        typeof(Style),
                                        typeof(SplitMenuButton),
                                        new FrameworkPropertyMetadata(default(Style)));

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (SplitMenuButton)d;
            owner.HasHeader = e.NewValue != null;
        }

        // ReSharper disable once NotAccessedField.Local
        private readonly DeepTracker _itemsSourcePropertyChangedTracker;
        private Button _actionButton;
        private Menu _menu;
        private ToggleButton _toggleButton;

        static SplitMenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitMenuButton), new FrameworkPropertyMetadata(typeof(SplitMenuButton)));
            ResourceHelper.SetDefaultStyle(typeof(SplitMenuButton), Resource.ControlStyle);
        }

        public SplitMenuButton()
        {
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ClickEventHandler));
            AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(CheckedUncheckedEventHandler));
            AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(CheckedUncheckedEventHandler));
            AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItemClickEventHandler));
            Items = new FreezableCollection<DependencyObject>();
            _itemsSourcePropertyChangedTracker = DeepTracker.Setup(this)
                                                            .Include(Route.Create(nameof(ItemsSource)))
                                                            .Include(Route.Create(nameof(Items)))
                                                            .Subscribe<ObjectAttachedEventArgs>(ItemsCollectionChanged)
                                                            .Create()
                                                            .Activate();
        }

        public bool HasHeader
        {
            get { return (bool)GetValue(HasHeaderProperty); }
            private set { SetValue(HasHeaderPropertyKey, value); }
        }

        public bool HasItems
        {
            get { return (bool)GetValue(HasItemsProperty); }
            private set { SetValue(HasItemsPropertyKey, value); }
        }

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public string HeaderStringFormat
        {
            get { return (string)GetValue(HeaderStringFormatProperty); }
            set { SetValue(HeaderStringFormatProperty, value); }
        }

        public System.Windows.DataTemplate HeaderTemplate
        {
            get { return (System.Windows.DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector HeaderTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }

        public bool IsMenuOpened
        {
            get { return (bool)GetValue(IsMenuOpenedProperty); }
            set { SetValue(IsMenuOpenedProperty, value); }
        }

        public FreezableCollection<DependencyObject> Items { get; }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Style MenuItemContainerStyle
        {
            get { return (Style)GetValue(MenuItemContainerStyleProperty); }
            set { SetValue(MenuItemContainerStyleProperty, value); }
        }

        public System.Windows.Controls.StyleSelector MenuItemContainerStyleSelector
        {
            get { return (System.Windows.Controls.StyleSelector)GetValue(MenuItemContainerStyleSelectorProperty); }
            set { SetValue(MenuItemContainerStyleSelectorProperty, value); }
        }

        public ItemsPanelTemplate MenuItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(MenuItemsPanelProperty); }
            set { SetValue(MenuItemsPanelProperty, value); }
        }

        public System.Windows.DataTemplate MenuItemTemplate
        {
            get { return (System.Windows.DataTemplate)GetValue(MenuItemTemplateProperty); }
            set { SetValue(MenuItemTemplateProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector MenuItemTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(MenuItemTemplateSelectorProperty); }
            set { SetValue(MenuItemTemplateSelectorProperty, value); }
        }

        public Style MenuStyle
        {
            get { return (Style)GetValue(MenuStyleProperty); }
            set { SetValue(MenuStyleProperty, value); }
        }

        [Category("Behavior")]
        public event RoutedEventHandler Checked
        {
            add { AddHandler(ToggleButton.CheckedEvent, value); }
            remove { RemoveHandler(ToggleButton.CheckedEvent, value); }
        }

        [Category("Behavior")]
        public event RoutedEventHandler Click
        {
            add { AddHandler(ButtonBase.ClickEvent, value); }
            remove { RemoveHandler(ButtonBase.ClickEvent, value); }
        }

        [Category("Behavior")]
        public event RoutedEventHandler Unchecked
        {
            add { AddHandler(ToggleButton.UncheckedEvent, value); }
            remove { RemoveHandler(ToggleButton.UncheckedEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_Button") is Button button)
            {
                _actionButton = button;
            }

            if (GetTemplateChild("PART_ToggleButton") is ToggleButton toggleButton)
            {
                _toggleButton = toggleButton;
            }

            if (GetTemplateChild("PART_Menu") is Menu menu)
            {
                _menu = menu;
                UpdateMenuItemsSource();
            }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        private void CheckedUncheckedEventHandler(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement frameworkElement && _toggleButton != null)
            {
                var toggleButtonSource = frameworkElement.AreEqual(_toggleButton) || frameworkElement.HasVisualParent(_toggleButton);
                //Suppress all except out toggle button check events
                e.Handled = !toggleButtonSource;
            }
        }

        private void ClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement frameworkElement && _actionButton != null)
            {
                var actionButtonSource = frameworkElement.AreEqual(_actionButton) || frameworkElement.HasVisualParent(_actionButton);
                //Suppress all except out action button clicks
                e.Handled = !actionButtonSource;
            }
        }

        private void ItemsCollectionChanged(object sender, ObjectAttachedEventArgs e)
        {
            if (e.Route.AreEqual(Route.Create(nameof(ItemsSource))))
            {
                UpdateMenuItemsSource();
            }
            else
            {
                UpdateHasItems();
            }
        }

        private void MenuItemClickEventHandler(object sender, RoutedEventArgs e)
        {
            IsMenuOpened = false;
        }

        private void UpdateHasItems()
        {
            HasItems = Items.Any() || ItemsSource?.Enumerate().Any() == true;
        }

        private void UpdateMenuItemsSource()
        {
            if (this.IsDefaultValue(ItemsSourceProperty))
            {
                _menu.ItemsSource = Items;
            }
            else
            {
                _menu.SetBinding(ItemsControl.ItemsSourceProperty,
                                 new Binding
                                 {
                                     Path = new PropertyPath(ItemsSourceProperty),
                                     Source = this
                                 });
            }

            UpdateHasItems();
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/SplitMenuButton/SplitMenuButton.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            public static readonly ResourceDescriptor ItemsPanelTemplate = ResourceDescriptor.Create<ItemsPanelTemplate>(Default);
            public static readonly ResourceDescriptor MenuItemStyle = ResourceDescriptor.Create<Style>(Default);
        }

        #endregion
    }
}
