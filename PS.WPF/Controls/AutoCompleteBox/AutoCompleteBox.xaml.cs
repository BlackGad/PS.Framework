using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using PS.Data;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Patterns.Command;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class AutoCompleteBox : BaseEditableBox

    {
        private static readonly DependencyPropertyKey AdditionalItemsSourcePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(AdditionalItemsSource),
                                                typeof(IEnumerable),
                                                typeof(AutoCompleteBox),
                                                new FrameworkPropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty AllowFreeItemProperty =
            DependencyProperty.Register(nameof(AllowFreeItem),
                                        typeof(bool),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty ClearFilterGeometryProperty =
            DependencyProperty.Register("ClearFilterGeometry",
                                        typeof(Geometry),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(Geometry)));

        public static readonly DependencyProperty FilterMemberPathProperty =
            DependencyProperty.Register("FilterMemberPath",
                                        typeof(string),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource",
                                        typeof(IEnumerable),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate),
                                        typeof(System.Windows.DataTemplate),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(System.Windows.DataTemplate)));

        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            DependencyProperty.Register(nameof(ItemTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty MaximumSuggestionCountProperty =
            DependencyProperty.Register("MaximumSuggestionCount",
                                        typeof(int),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(int)));

        public static readonly DependencyProperty PopupHeightProperty =
            DependencyProperty.Register("PopupHeight",
                                        typeof(double),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(double), null, OnPopupHeightCoerce));

        public static readonly DependencyProperty PopupItemTemplateProperty =
            DependencyProperty.Register("PopupItemTemplate",
                                        typeof(System.Windows.DataTemplate),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(System.Windows.DataTemplate)));

        public static readonly DependencyProperty PopupItemTemplateSelectorProperty =
            DependencyProperty.Register("PopupItemTemplateSelector",
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register("PopupPlacement",
                                        typeof(PlacementMode),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(PlacementMode)));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem",
                                        typeof(object),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnSelectedItemChanged));

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue",
                                        typeof(object),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default,
                                                                      FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                      OnSelectedValueChanged));

        public static readonly DependencyProperty SelectFirstItemOnResetProperty =
            DependencyProperty.Register(nameof(SelectFirstItemOnReset),
                                        typeof(bool),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty ShowItemsWhenHasSuggestionsProperty =
            DependencyProperty.Register(nameof(ShowItemsWhenHasSuggestions),
                                        typeof(bool),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(bool)));

        public static readonly DependencyProperty ValueMemberPathProperty =
            DependencyProperty.Register("ValueMemberPath",
                                        typeof(string),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(string)));

        internal static readonly DependencyProperty AdditionalItemsSourceProperty = AdditionalItemsSourcePropertyKey.DependencyProperty;

        internal static readonly DependencyProperty ClearFilterCommandProperty =
            DependencyProperty.Register("ClearFilterCommand",
                                        typeof(ICommand),
                                        typeof(AutoCompleteBox),
                                        new FrameworkPropertyMetadata(default(ICommand)));

        public static readonly RoutedEvent PreviewItemSelectionEvent = EventManager.RegisterRoutedEvent(
            "PreviewItemSelection",
            RoutingStrategy.Bubble,
            typeof(PreviewItemSelectionEventHandler),
            typeof(AutoCompleteBox));

        private static object OnPopupHeightCoerce(DependencyObject d, object baseValue)
        {
            if (baseValue is double newValue)
            {
                return Math.Max(0, newValue);
            }

            return baseValue;
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (AutoCompleteBox)d;
            owner.UpdateSelectedValue();
        }

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (AutoCompleteBox)d;
            owner.UpdateSelectedItem();
        }

        private readonly Flag _selectedPropertiesBalancing;
        private Popup _popup;
        private SuggestListView _suggestListView;
        private System.Windows.Controls.TextBox _textBox;

        static AutoCompleteBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteBox), new FrameworkPropertyMetadata(typeof(AutoCompleteBox)));
            ResourceHelper.SetDefaultStyle(typeof(AutoCompleteBox), Resource.ControlStyle);
        }

        public AutoCompleteBox()
        {
            Loaded += OnLoaded;
            _selectedPropertiesBalancing = new Flag();
            ClearFilterCommand = new RelayUICommand(OnClearFilter)
            {
                Title = "Clear filter"
            };
        }

        public bool AllowFreeItem
        {
            get { return (bool)GetValue(AllowFreeItemProperty); }
            set { SetValue(AllowFreeItemProperty, value); }
        }

        public Geometry ClearFilterGeometry
        {
            get { return (Geometry)GetValue(ClearFilterGeometryProperty); }
            set { SetValue(ClearFilterGeometryProperty, value); }
        }

        public string FilterMemberPath
        {
            get { return (string)GetValue(FilterMemberPathProperty); }
            set { SetValue(FilterMemberPathProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public System.Windows.DataTemplate ItemTemplate
        {
            get { return (System.Windows.DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector ItemTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        public int MaximumSuggestionCount
        {
            get { return (int)GetValue(MaximumSuggestionCountProperty); }
            set { SetValue(MaximumSuggestionCountProperty, value); }
        }

        public double PopupHeight
        {
            get { return (double)GetValue(PopupHeightProperty); }
            set { SetValue(PopupHeightProperty, value); }
        }

        public System.Windows.DataTemplate PopupItemTemplate
        {
            get { return (System.Windows.DataTemplate)GetValue(PopupItemTemplateProperty); }
            set { SetValue(PopupItemTemplateProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector PopupItemTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(PopupItemTemplateSelectorProperty); }
            set { SetValue(PopupItemTemplateSelectorProperty, value); }
        }

        public PlacementMode PopupPlacement
        {
            get { return (PlacementMode)GetValue(PopupPlacementProperty); }
            set { SetValue(PopupPlacementProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public bool SelectFirstItemOnReset
        {
            get { return (bool)GetValue(SelectFirstItemOnResetProperty); }
            set { SetValue(SelectFirstItemOnResetProperty, value); }
        }

        public bool ShowItemsWhenHasSuggestions
        {
            get { return (bool)GetValue(ShowItemsWhenHasSuggestionsProperty); }
            set { SetValue(ShowItemsWhenHasSuggestionsProperty, value); }
        }

        public string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { SetValue(ValueMemberPathProperty, value); }
        }

        internal IEnumerable AdditionalItemsSource
        {
            get { return (IEnumerable)GetValue(AdditionalItemsSourceProperty); }
            private set { SetValue(AdditionalItemsSourcePropertyKey, value); }
        }

        internal ICommand ClearFilterCommand
        {
            get { return (ICommand)GetValue(ClearFilterCommandProperty); }
            set { SetValue(ClearFilterCommandProperty, value); }
        }

        public event PreviewItemSelectionEventHandler PreviewItemSelection
        {
            add { AddHandler(PreviewItemSelectionEvent, value); }
            remove { RemoveHandler(PreviewItemSelectionEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = GetTemplateChild("PART_Popup") as Popup;
            _textBox = GetTemplateChild("PART_TextBox") as System.Windows.Controls.TextBox;
            _suggestListView = GetTemplateChild("PART_SuggestList") as SuggestListView;
        }

        protected override string FormatValueToDisplayText(string formatString, CultureInfo cultureInfo)
        {
            var effectiveValue = SelectedItem.GetEffectiveValue(FilterMemberPath);
            var filter = effectiveValue.GetEffectiveString();

            return string.Format(FormatString ?? "{0}", filter);
        }

        protected override bool HasValue()
        {
            return SelectedItem != null;
        }

        protected override void OnBeginEdit()
        {
            if (_textBox != null)
            {
                var effectiveValue = SelectedItem.GetEffectiveValue(FilterMemberPath);
                var filter = effectiveValue.GetEffectiveString();

                _textBox.Text = filter;
                _textBox.Focus();
                _textBox.SelectAll();
            }

            if (_suggestListView != null && SelectedItem != null)
            {
                using (_selectedPropertiesBalancing.Scope())
                {
                    //Remove additional items
                    AdditionalItemsSource = null;

                    //Try to set current item to suggest view
                    _suggestListView.SelectedItem = SelectedItem;

                    //If item differs from selected put it to additional items and reselect
                    if (!_suggestListView.SelectedItem.AreEqual(SelectedItem))
                    {
                        AdditionalItemsSource = new[] { SelectedItem };
                        _suggestListView.SelectedItem = SelectedItem;
                    }
                }
            }
        }

        protected override void OnEndEdit(EndEditReason reason)
        {
            if (_suggestListView == null) return;

            if (reason == EndEditReason.ReturnPressed && _suggestListView.SelectedItem == null && !AllowFreeItem)
            {
                _suggestListView.SelectNext();
            }

            var args = new PreviewItemSelectionEventArgs(_suggestListView.Input, PreviewItemSelectionEvent, this)
            {
                Item = _suggestListView.SelectedItem
            };

            RaiseEvent(args);

            AdditionalItemsSource = null;

            if (args.Item != null && (_suggestListView.Items.Contains(args.Item) || AllowFreeItem))
            {
                AdditionalItemsSource = new[] { args.Item };
            }

            SelectedItem = args.Item;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    _suggestListView?.SelectNext();
                    break;
                case Key.Up:
                    _suggestListView?.SelectPrevious();
                    break;
                case Key.PageDown:
                    _suggestListView?.SelectNext(true);
                    break;
                case Key.PageUp:
                    _suggestListView?.SelectPrevious(true);
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element)
            {
                if (_popup?.HasVisualParent(element) == true) return;
            }

            base.OnMouseDown(e);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject source)
            {
                if (_suggestListView.HasVisualParent(source) && !source.HasVisualParentOf(typeof(ScrollBar)))
                {
                    Dispatcher.Postpone(() => EndEdit(EndEditReason.Focus));
                }
            }

            base.OnPreviewMouseDown(e);
        }
        
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                UpdateSelectedValue();
            }
            else if (SelectedValue != null)
            {
                UpdateSelectedItem();
            }
        }

        private void OnClearFilter()
        {
            if (_suggestListView != null)
            {
                _suggestListView.Input = string.Empty;
                _suggestListView.SelectedItem = null;
            }

            EndEdit();
        }

        private void UpdateSelectedItem()
        {
            if (_selectedPropertiesBalancing) return;
            if (!IsLoaded) return;

            using (_selectedPropertiesBalancing.Scope())
            {
                SelectedItem = ItemsSource.Enumerate()
                                          .FirstOrDefault(i => i.GetEffectiveValue(ValueMemberPath)
                                                                .AreEqual(SelectedValue));
            }

            NotifyValueChanged();
        }

        private void UpdateSelectedValue()
        {
            if (_selectedPropertiesBalancing) return;
            if (!IsLoaded) return;

            using (_selectedPropertiesBalancing.Scope())
            {
                SelectedValue = SelectedItem.GetEffectiveValue(ValueMemberPath);
            }

            NotifyValueChanged();
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/AutoCompleteBox/AutoCompleteBox.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ClearFilterGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Default geometry for clear filter button",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default AutoCompleteBox style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default AutoCompleteBox control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
