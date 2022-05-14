using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.Extensions;
using PS.WPF.Docking.Components;
using PS.WPF.Docking.Layout;
using PS.WPF.Resources;

namespace PS.WPF.Docking.Controls
{
    [ContentProperty(nameof(Areas))]
    public class DockingManager : Control
    {
        #region Property definitions

        private static readonly DependencyPropertyKey ItemsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Areas),
                                                typeof(FreezableCollection<DockingArea>),
                                                typeof(DockingManager),
                                                new FrameworkPropertyMetadata(default(FreezableCollection<DockingArea>)));

        public static readonly DependencyProperty AreasProperty = ItemsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty DragOperationAreaLayoutSnapshotProperty =
            DependencyProperty.RegisterAttached("DragOperationAreaLayoutSnapshot",
                                                typeof(DragOperationAreaLayoutSnapshot),
                                                typeof(DockingManager),
                                                new PropertyMetadata(default(DragOperationAreaLayoutSnapshot)));

        public static readonly DependencyProperty HostProperty =
            DependencyProperty.RegisterAttached("Host",
                                                typeof(DockingHostDescriptor),
                                                typeof(DockingManager),
                                                new PropertyMetadata(OnAreaHostChanged));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource),
                                        typeof(IEnumerable),
                                        typeof(DockingManager),
                                        new FrameworkPropertyMetadata(null, OnItemsSourceCoerce));

        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            DependencyProperty.Register(nameof(ItemTemplateSelector),
                                        typeof(System.Windows.Controls.DataTemplateSelector),
                                        typeof(DockingManager),
                                        new FrameworkPropertyMetadata(default(System.Windows.Controls.DataTemplateSelector)));

        public static readonly DependencyProperty ManagerProperty =
            DependencyProperty.RegisterAttached("Manager",
                                                typeof(DockingManager),
                                                typeof(DockingManager),
                                                new PropertyMetadata(OnAreaDockingManagerChanged));

        #endregion

        #region Constants

        public static readonly DockingHostDescriptor DraggedHostDescriptor;

        public static readonly DockingHostDescriptor InternalHostDescriptor;

        #endregion

        #region Static members

        public static DragOperationAreaLayoutSnapshot GetDragOperationAreaLayoutSnapshot(DependencyObject element)
        {
            return (DragOperationAreaLayoutSnapshot)element.GetValue(DragOperationAreaLayoutSnapshotProperty);
        }

        public static DockingHostDescriptor GetHost(DependencyObject element)
        {
            return (DockingHostDescriptor)element.GetValue(HostProperty);
        }

        public static DockingManager GetManager(DependencyObject element)
        {
            return (DockingManager)element.GetValue(ManagerProperty);
        }

        public static void SetDragOperationAreaLayoutSnapshot(DependencyObject element, DragOperationAreaLayoutSnapshot value)
        {
            element.SetValue(DragOperationAreaLayoutSnapshotProperty, value);
        }

        public static void SetHost(DependencyObject element, DockingHostDescriptor value)
        {
            element.SetValue(HostProperty, value);
        }

        public static void SetManager(DependencyObject element, DockingManager value)
        {
            element.SetValue(ManagerProperty, value);
        }

        public static void StartDraggingOperation(DockingArea area)
        {
            if (area == null) throw new ArgumentNullException(nameof(area));
            SetDragOperationAreaLayoutSnapshot(area,
                                               new DragOperationAreaLayoutSnapshot
                                               {
                                                   ActualWidth = area.ActualWidth,
                                                   ActualHeight = area.ActualHeight,
                                                   AbsolutePosition = area.PointToScreen(new Point(0, 0))
                                               });

            SetHost(area, DraggedHostDescriptor);
        }

        private static void OnAreaDockingManagerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (e.OldValue is DockingManager oldValue)
                {
                    oldValue.GetHostAreas(GetHost(d)).Remove(element);
                }

                if (e.NewValue is DockingManager newValue)
                {
                    newValue.GetHostAreas(GetHost(d)).Add(element);
                }
            }
        }

        private static void OnAreaHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                var manager = GetManager(d);
                if (manager == null)
                {
                    //Area is not attached to any host
                    return;
                }

                manager.GetHostAreas(e.OldValue as DockingHostDescriptor).Remove(element);
                manager.GetHostAreas(e.NewValue as DockingHostDescriptor).Add(element);
            }
        }

        private static object OnItemsSourceCoerce(DependencyObject d, object baseValue)
        {
            return baseValue ?? new ObservableCollection<object>();
        }

        #endregion

        private readonly Dictionary<DockingHostDescriptor, ObservableCollection<FrameworkElement>> _hostsAreas;

        private readonly DeepTracker _itemsSourceTracker;
        private DockingLayout _layout;

        #region Constructors

        static DockingManager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockingManager), new FrameworkPropertyMetadata(typeof(DockingManager)));
            ResourceHelper.SetDefaultStyle(typeof(DockingManager), Resource.ControlStyle);

            InternalHostDescriptor = new DockingHostDescriptor();
            DraggedHostDescriptor = new DockingHostDescriptor("384012E55C1C400D87CD40C24997FAF0");
        }

        public DockingManager()
        {
            _itemsSourceTracker = DeepTracker.Setup(this, Route.Create(nameof(ItemsSource), Routes.Wildcard))
                                             .Subscribe<ObjectAttachmentEventArgs>(OnItemAttachment)
                                             .Create();
            CoerceValue(ItemsSourceProperty);

            _layout = new DockingLayout();

            Areas = new FreezableCollection<DockingArea>();
            CollectionChangedEventManager.AddHandler(Areas, AreasOnChanged);

            _hostsAreas = new Dictionary<DockingHostDescriptor, ObservableCollection<FrameworkElement>>();
        }

        #endregion

        #region Properties

        public FreezableCollection<DockingArea> Areas
        {
            get { return (FreezableCollection<DockingArea>)GetValue(AreasProperty); }
            private set { SetValue(ItemsPropertyKey, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public System.Windows.Controls.DataTemplateSelector ItemTemplateSelector
        {
            get { return (System.Windows.Controls.DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        #endregion

        #region Override members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Host") is ItemsControl itemsControl)
            {
                itemsControl.ItemsSource = GetHostAreas();
            }

            _itemsSourceTracker.Activate();
        }

        #endregion

        #region Event handlers

        private void AreasOnChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems.Enumerate<DependencyObject>();
            var oldItems = e.OldItems.Enumerate<DependencyObject>();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in newItems)
                    {
                        SetManager(newItem, this);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in oldItems)
                    {
                        SetManager(oldItem, null);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (var oldItem in oldItems)
                    {
                        SetManager(oldItem, null);
                    }

                    foreach (var newItem in newItems)
                    {
                        SetManager(newItem, this);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    //TODO: What to do?
                    break;
            }
        }

        private void OnItemAttachment(object sender, ObjectAttachmentEventArgs e)
        {
            if (!e.Route.IsDynamic()) return;

            if (e is ObjectAttachedEventArgs)
            {
                var container = e.Object is DockingArea area ? area : new DockingArea();

                container.Content = e.Object;
                Areas.Add(container);
            }

            if (e is ObjectDetachedEventArgs)
            {
                var container = Areas.FirstOrDefault(i => i.Content.AreEqual(e.Object));
                if (container == null) return;

                Areas.Remove(container);
                container.Content = null;
            }
        }

        #endregion

        #region Members

        private IList<FrameworkElement> GetHostAreas(DockingHostDescriptor descriptor = null)
        {
            descriptor = descriptor ?? InternalHostDescriptor;
            if (!_hostsAreas.ContainsKey(descriptor))
            {
                var areaCollection = descriptor.AreEqual(InternalHostDescriptor)
                    ? new ObservableCollection<FrameworkElement>()
                    : new DockingHostAreaCollection();

                _hostsAreas.Add(descriptor, areaCollection);
            }

            return _hostsAreas[descriptor];
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default = new Uri("/PS.WPF.Docking;component/Controls/DockingManager.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
            public static readonly ResourceDescriptor ControlTemplate = ResourceDescriptor.Create<ControlTemplate>(Default);

            #endregion
        }

        #endregion
    }

    public class DragOperationAreaLayoutSnapshot
    {
        #region Properties

        public Point AbsolutePosition { get; set; }
        public double ActualHeight { get; set; }
        public double ActualWidth { get; set; }

        #endregion
    }
}