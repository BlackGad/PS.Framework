using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.Data.Fuzzy;
using PS.Data.Fuzzy.Computers;
using PS.Extensions;
using PS.Extensions.Ducking;
using PS.Threading.ThrottlingTrigger;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class SuggestListView : ListView
    {
        public static readonly DependencyProperty ComputerProperty =
            DependencyProperty.Register(nameof(Computer),
                                        typeof(FuzzyCoefficientComputer),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(OnComputerChanged, OnComputerCoerce));

        public static readonly DependencyProperty FilterMemberPathProperty =
            DependencyProperty.Register("FilterMemberPath",
                                        typeof(string),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(string)));

        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer",
                                        typeof(object),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty FooterTemplateProperty =
            DependencyProperty.Register("FooterTemplate",
                                        typeof(System.Windows.DataTemplate),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(System.Windows.DataTemplate)));

        public static readonly DependencyProperty FooterTemplateSelectorProperty =
            DependencyProperty.Register("FooterTemplateSelector",
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header",
                                        typeof(object),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(object)));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate",
                                        typeof(System.Windows.DataTemplate),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(System.Windows.DataTemplate)));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
            DependencyProperty.Register("HeaderTemplateSelector",
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register("Input",
                                        typeof(string),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(OnInputChanged));

        public static readonly DependencyProperty MaximumSuggestionCountProperty =
            DependencyProperty.Register("MaximumSuggestionCount",
                                        typeof(int),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(int)));

        public static readonly DependencyProperty SelectFirstItemOnResetProperty =
            DependencyProperty.Register(nameof(SelectFirstItemOnReset),
                                        typeof(bool),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty ShowItemsWhenHasSuggestionsProperty =
            DependencyProperty.Register("ShowItemsWhenHasSuggestions",
                                        typeof(bool),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty SuggestedItemsProperty =
            DependencyProperty.Register("SuggestedItems",
                                        typeof(IEnumerable),
                                        typeof(SuggestListView),
                                        new FrameworkPropertyMetadata(default(IEnumerable), OnSuggestedItemsChanged, OnSuggestedItemsCoerce));

        private static void OnComputerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (SuggestListView)d;
            owner.UpdateSuggestions();
        }

        private static object OnComputerCoerce(DependencyObject d, object baseValue)
        {
            return baseValue ?? FuzzyCoefficientComputer.Setup()
                                                        .Use<LevenshteinDistanceComputer>()
                                                        .Use<TanimotoCoefficientComputer>()
                                                        .Use<OverlapCoefficientComputer>()
                                                        .Use<SequencePositionCoefficientComputer>(0.5)
                                                        .Create();
        }

        private static void OnInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (SuggestListView)d;
            owner.UpdateSuggestions();
        }

        private static void OnSuggestedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (SuggestListView)d;
            owner.UpdateSuggestions();
        }

        private static object OnSuggestedItemsCoerce(DependencyObject d, object baseValue)
        {
            return baseValue ?? new ObservableCollection<object>();
        }

        private readonly ThrottlingTrigger _itemSourceChangedThrottlingTrigger;
        private readonly DeepTracker _itemSourceTracker;

        private ListView _suggestionsListView;

        static SuggestListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuggestListView), new FrameworkPropertyMetadata(typeof(SuggestListView)));
            ResourceHelper.SetDefaultStyle(typeof(SuggestListView), Resource.ControlStyle);
        }

        public SuggestListView()
        {
            CoerceValue(ComputerProperty);

            _itemSourceTracker = DeepTracker.Setup(this)
                                            .Include<SuggestListView>(instance => instance.ItemsSource)
                                            .Subscribe<ChangedEventArgs>(ItemsSourceChanged)
                                            .Create()
                                            .Activate();

            _itemSourceChangedThrottlingTrigger = ThrottlingTrigger.Setup()
                                                                   .Throttle(TimeSpan.FromMilliseconds(50))
                                                                   .DispatchWith(new DispatcherSynchronizationContext(Dispatcher))
                                                                   .Subscribe<EventArgs>(OnItemSourceChangedThrottlingTrigger)
                                                                   .Create()
                                                                   .Activate();

            AddHandler(MouseDownEvent, new MouseButtonEventHandler(OnMouseDown));
            Loaded += (sender, args) => ResetSelection();
            CoerceValue(SuggestedItemsProperty);
        }

        public FuzzyCoefficientComputer Computer
        {
            get { return (FuzzyCoefficientComputer)GetValue(ComputerProperty); }
            set { SetValue(ComputerProperty, value); }
        }

        public string FilterMemberPath
        {
            get { return (string)GetValue(FilterMemberPathProperty); }
            set { SetValue(FilterMemberPathProperty, value); }
        }

        public object Footer
        {
            get { return GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public System.Windows.DataTemplate FooterTemplate
        {
            get { return (System.Windows.DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector FooterTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(FooterTemplateSelectorProperty); }
            set { SetValue(FooterTemplateSelectorProperty, value); }
        }

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
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

        public string Input
        {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public int MaximumSuggestionCount
        {
            get { return (int)GetValue(MaximumSuggestionCountProperty); }
            set { SetValue(MaximumSuggestionCountProperty, value); }
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

        public IEnumerable SuggestedItems
        {
            get { return (IEnumerable)GetValue(SuggestedItemsProperty); }
            set { SetValue(SuggestedItemsProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _suggestionsListView = GetTemplateChild("PART_Suggestions") as ListView;
            if (_suggestionsListView != null) _suggestionsListView.SelectionChanged += SuggestionsListViewOnSelectionChanged;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.AreEqual(FilterMemberPathProperty))
            {
                using (Items.DeferRefresh())
                {
                    Items.SortDescriptions.Clear();
                    if (e.NewValue is string newValue && !string.IsNullOrEmpty(newValue))
                    {
                        Items.SortDescriptions.Add(new SortDescription(newValue, ListSortDirection.Ascending));
                    }
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SuggestListViewItem();
        }

        private void ItemsSourceChanged(object sender, ChangedEventArgs e)
        {
            _itemSourceChangedThrottlingTrigger.Trigger();
        }

        private void OnItemSourceChangedThrottlingTrigger(object sender, EventArgs e)
        {
            UpdateSuggestions();
            ResetSelection();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && ItemsSource.Enumerate().Contains(element.DataContext))
            {
                SelectedItem = element.DataContext;
            }
        }

        private void SuggestionsListViewOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Enumerate().Any() && _suggestionsListView != null)
            {
                SelectedItem = _suggestionsListView.SelectedItem;
            }
        }

        public void SelectNext(bool jump = false)
        {
            var deltaIndex = jump ? 10 : 1;

            if (_suggestionsListView?.HasItems == true)
            {
                var index = _suggestionsListView.SelectedIndex;
                _suggestionsListView.SelectedIndex = Math.Min(index + deltaIndex, _suggestionsListView.Items.Count - 1);
            }
            else if (HasItems)
            {
                var index = SelectedIndex;
                SelectedIndex = Math.Min(index + deltaIndex, Items.Count - 1);
            }

            ScrollIntoView(SelectedItem);
        }

        public void SelectPrevious(bool jump = false)
        {
            var deltaIndex = jump ? 10 : 1;

            if (_suggestionsListView?.HasItems == true)
            {
                var index = _suggestionsListView.SelectedIndex;
                _suggestionsListView.SelectedIndex = Math.Max(index - deltaIndex, 0);
            }
            else if (HasItems)
            {
                var index = SelectedIndex;
                SelectedIndex = Math.Max(index - deltaIndex, 0);
            }

            ScrollIntoView(SelectedItem);
        }

        private void ResetSelection()
        {
            if (SelectFirstItemOnReset)
            {
                if (_suggestionsListView?.HasItems == true)
                {
                    _suggestionsListView.SelectedIndex = 0;
                }
                else if (HasItems)
                {
                    SelectedIndex = 0;
                }

                ScrollIntoView(SelectedItem);
            }
            else
            {
                SelectedItem = null;
            }
        }

        private void UpdateSuggestions()
        {
            var effectiveValues = Items.Enumerate()
                                       .Select(item =>
                                       {
                                           var effectiveValue = item.GetEffectiveValue(FilterMemberPath);
                                           var filter = effectiveValue.GetEffectiveString();

                                           if (effectiveValue != null)
                                           {
                                               var valueType = effectiveValue.GetType();
                                               filter = TypeDescriptor.GetConverter(valueType)
                                                                      .ConvertToString(effectiveValue);
                                           }

                                           return new
                                           {
                                               Item = item,
                                               Value = effectiveValue,
                                               Filter = filter
                                           };
                                       })
                                       .Where(s => s.Value != null)
                                       .ToLookup(s => s, s => 0d)
                                       .ToDictionary(g => g.Key, g => g.First());

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4
            };

            var input = Input;
            var computer = Computer;

            Parallel.ForEach(effectiveValues.ToList(),
                             options,
                             pair =>
                             {
                                 var coefficient = computer.Compute(input, pair.Key.Filter);
                                 effectiveValues[pair.Key] = coefficient;
                             });

            foreach (var item in SuggestedItems.Enumerate().ToList())
            {
                SuggestedItems.CollectionRemove(item);
            }

            var suggestions = effectiveValues.Where(pair => pair.Value > 0.5)
                                             .OrderByDescending(pair => pair.Value)
                                             .Take(Math.Max(MaximumSuggestionCount, 0))
                                             .Select(pair => pair.Key.Item);

            foreach (var item in suggestions)
            {
                SuggestedItems.CollectionAdd(item);
            }

            ResetSelection();
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/SuggestListView/SuggestListView.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default SuggestListView style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default SuggestListView control template",
                                                           resourceDictionary: Default);

            public static readonly ResourceDescriptor SuggestionItemContainerStyle =
                ResourceDescriptor.Create<Style>(description: "Default Suggestion item container style",
                                                 resourceDictionary: Default);
        }

        #endregion
    }
}
