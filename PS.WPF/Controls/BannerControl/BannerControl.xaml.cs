using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.BannerControl
{
    public class BannerControl : ItemsControl
    {
        public static readonly DependencyProperty ColumnDefinitionsProperty =
            DependencyProperty.Register(nameof(ColumnDefinitions),
                                        typeof(FreezableCollection<ColumnDefinition>),
                                        typeof(BannerControl),
                                        new FrameworkPropertyMetadata(default(FreezableCollection<ColumnDefinition>)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius),
                                        typeof(CornerRadius),
                                        typeof(BannerControl),
                                        new FrameworkPropertyMetadata(OnCornerRadiusPropertyChanged));

        public static readonly DependencyProperty LayoutFlowDirectionProperty =
            DependencyProperty.Register(nameof(LayoutFlowDirection),
                                        typeof(FlowDirection),
                                        typeof(BannerControl),
                                        new FrameworkPropertyMetadata(OnFlowDirectionPropertyChanged));

        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BannerControl)d;
            owner.UpdateItems();
        }

        private static void OnFlowDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BannerControl)d;
            owner.UpdatePanel();
            owner.UpdateItems();
        }

        private Grid _mainPanel;

        static BannerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BannerControl), new FrameworkPropertyMetadata(typeof(BannerControl)));
            ResourceHelper.SetDefaultStyle(typeof(BannerControl), Resource.ControlStyle);
        }

        public BannerControl()
        {
            ColumnDefinitions = new FreezableCollection<ColumnDefinition>();
            AddHandler(BannerControlPanel.PanelLoadedEvent, new RoutedEventHandler(OnMainPanelLoaded));
            AddHandler(BannerControlPanel.PanelChildChangedEvent, new RoutedEventHandler(OnMainPanelChildChanged));
        }

        public FreezableCollection<ColumnDefinition> ColumnDefinitions
        {
            get { return (FreezableCollection<ColumnDefinition>)GetValue(ColumnDefinitionsProperty); }
            set { SetValue(ColumnDefinitionsProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public FlowDirection LayoutFlowDirection
        {
            get { return (FlowDirection)GetValue(LayoutFlowDirectionProperty); }
            set { SetValue(LayoutFlowDirectionProperty, value); }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var result = base.ArrangeOverride(arrangeBounds);
            UpdateItems();
            return result;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BannerControlItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is BannerControlItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            element.SetBindingIfDefault(BorderBrushProperty,
                                        new Binding
                                        {
                                            Source = this,
                                            Path = new PropertyPath(BorderBrushProperty),
                                        });

            Dispatcher.Postpone(UpdateItems);
        }

        private void OnMainPanelChildChanged(object sender, RoutedEventArgs e)
        {
            UpdateItems();
        }

        private void OnMainPanelLoaded(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Grid grid)
            {
                _mainPanel = grid;

                UpdatePanel();
                UpdateItems();
            }
        }

        private void UpdateItems()
        {
            var itemContainers = ItemContainerGenerator
                                 .Items
                                 .Enumerate()
                                 .Select(item => ItemContainerGenerator.ContainerFromItem(item))
                                 .OfType<BannerControlItem>()
                                 .ToList();

            var firstVisibleContainer = itemContainers.FirstOrDefault(c => c.Visibility != Visibility.Collapsed);
            var lastVisibleContainer = itemContainers.LastOrDefault(c => c.Visibility != Visibility.Collapsed);

            foreach (var itemContainer in itemContainers)
            {
                var containerIndex = ItemContainerGenerator.IndexFromContainer(itemContainer);
                var columnIndex = LayoutFlowDirection == FlowDirection.LeftToRight
                    ? containerIndex
                    : ItemContainerGenerator.Items.Count - containerIndex - 1;

                Grid.SetColumn(itemContainer, columnIndex);

                var isFirstItem = itemContainer == firstVisibleContainer;
                var isLastItem = itemContainer == lastVisibleContainer;

                if (isFirstItem && isLastItem)
                {
                    itemContainer.CornerRadius = CornerRadius;
                }
                else if (isFirstItem && LayoutFlowDirection == FlowDirection.LeftToRight)
                {
                    itemContainer.CornerRadius = new CornerRadius(CornerRadius.TopLeft, 0, 0, CornerRadius.BottomLeft);
                }
                else if (isFirstItem && LayoutFlowDirection == FlowDirection.RightToLeft)
                {
                    itemContainer.CornerRadius = new CornerRadius(0, CornerRadius.TopLeft, CornerRadius.BottomLeft, 0);
                }
                else if (isLastItem && LayoutFlowDirection == FlowDirection.LeftToRight)
                {
                    itemContainer.CornerRadius = new CornerRadius(0, CornerRadius.TopRight, CornerRadius.BottomRight, 0);
                }
                else if (isLastItem && LayoutFlowDirection == FlowDirection.RightToLeft)
                {
                    itemContainer.CornerRadius = new CornerRadius(CornerRadius.TopRight, 0, 0, CornerRadius.BottomRight);
                }
                else
                {
                    itemContainer.CornerRadius = new CornerRadius();
                }

                if (isFirstItem && isLastItem)
                {
                    itemContainer.BorderThickness = BorderThickness;
                }
                else if (isFirstItem && LayoutFlowDirection == FlowDirection.LeftToRight)
                {
                    itemContainer.BorderThickness = new Thickness(BorderThickness.Left, BorderThickness.Top, 0, BorderThickness.Bottom);
                }
                else if (isFirstItem && LayoutFlowDirection == FlowDirection.RightToLeft)
                {
                    itemContainer.BorderThickness = new Thickness(0, BorderThickness.Top, BorderThickness.Left, BorderThickness.Bottom);
                }
                else if (isLastItem && LayoutFlowDirection == FlowDirection.LeftToRight)
                {
                    itemContainer.BorderThickness = new Thickness(0, BorderThickness.Top, BorderThickness.Right, BorderThickness.Bottom);
                }
                else if (isLastItem && LayoutFlowDirection == FlowDirection.RightToLeft)
                {
                    itemContainer.BorderThickness = new Thickness(BorderThickness.Right, BorderThickness.Top, 0, BorderThickness.Bottom);
                }
                else
                {
                    itemContainer.BorderThickness = new Thickness(0, BorderThickness.Top, 0, BorderThickness.Bottom);
                }
            }
        }

        private void UpdatePanel()
        {
            if (_mainPanel == null)
            {
                return;
            }

            _mainPanel.ColumnDefinitions.Clear();

            var definitions = LayoutFlowDirection == FlowDirection.LeftToRight
                ? ColumnDefinitions.AsEnumerable()
                : ColumnDefinitions.Reverse();

            _mainPanel.ColumnDefinitions.Clear();
            foreach (var definition in definitions.Enumerate())
            {
                _mainPanel.ColumnDefinitions.Add(definition);
            }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/BannerControl/BannerControl.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);
        }

        #endregion
    }
}
