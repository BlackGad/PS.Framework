using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.Extensions;
using PS.Shell.Module.Diagram.Controls.MVVM;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.Shell.Module.Diagram.Controls
{
    public class DiagramWorkspace : ItemsControl
    {
        #region Property definitions

        public static readonly DependencyProperty DiagramProperty =
            DependencyProperty.Register(nameof(Diagram),
                                        typeof(Diagram),
                                        typeof(DiagramWorkspace),
                                        new FrameworkPropertyMetadata(default(Diagram)));

        public static readonly DependencyProperty ViewBoxOffsetProperty =
            DependencyProperty.Register(nameof(ViewBoxOffset),
                                        typeof(Point),
                                        typeof(DiagramWorkspace),
                                        new FrameworkPropertyMetadata(default(Point)));

        #endregion

        #region Constants

        private static readonly Route QueryGraphRoute;

        private static readonly Route QueryNodeRoute;
        private static readonly Route QueryNodeVisualGeometryRoute;
        private static readonly Route QueryNodeVisualSelectionRoute;

        #endregion

        private readonly DeepTracker _graphNodesTracker;
        private readonly DeepTracker _selectedItemsTracker;

        private DragOperation _dragOperation;
        private object _lastOverrideItem;

        #region Constructors

        static DiagramWorkspace()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramWorkspace), new FrameworkPropertyMetadata(typeof(DiagramWorkspace)));
            ResourceHelper.SetDefaultStyle(typeof(DiagramWorkspace), Resource.ControlStyle);

            QueryGraphRoute = Route.Create(nameof(Diagram), nameof(Controls.Diagram.Graph));
            QueryNodeRoute = Route.Create(QueryGraphRoute, nameof(IDiagramGraph.Vertices), Routes.Wildcard);
            QueryNodeVisualSelectionRoute = Route.Create(QueryNodeRoute, nameof(INode.Visual), nameof(INodeVisual.IsSelected));
            QueryNodeVisualGeometryRoute = Route.Create(QueryNodeRoute, nameof(INode.Geometry), Routes.Wildcard);
        }

        public DiagramWorkspace()
        {
            AddHandler(MouseDownEvent, new MouseButtonEventHandler(OnMouseDown));
            AddHandler(MouseMoveEvent, new MouseEventHandler(OnMouseMove));
            AddHandler(MouseUpEvent, new MouseButtonEventHandler(OnMouseUp));
            AddHandler(LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture));

            _selectedItemsTracker = DeepTracker.Setup(this, nameof(Diagram), nameof(Controls.Diagram.SelectedObjects), Routes.Wildcard)
                                               .Subscribe<ObjectAttachmentEventArgs>(OnSelectedObjectsAttachment)
                                               .Create();

            _graphNodesTracker = DeepTracker.Setup(this, QueryGraphRoute, Routes.WildcardRecursive)
                                            .Exclude<INode>(node => node.ViewModel)
                                            .Subscribe<ObjectAttachmentEventArgs>(OnVertexAttachment)
                                            .Subscribe<ChangedPropertyEventArgs>(OnVertexPropertyChanged)
                                            .Create();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        #endregion

        #region Properties

        public Diagram Diagram
        {
            get { return (Diagram)GetValue(DiagramProperty); }
            set { SetValue(DiagramProperty, value); }
        }

        public Point ViewBoxOffset
        {
            get { return (Point)GetValue(ViewBoxOffsetProperty); }
            set { SetValue(ViewBoxOffsetProperty, value); }
        }

        #endregion

        #region Override members

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            _lastOverrideItem = item;
            return item is Node || item is Connector;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            if (_lastOverrideItem is INode) return new Node();
            if (_lastOverrideItem is IConnector) return new Connector();
            throw new NotSupportedException("Unknown item type");
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element is Node node)
            {
                node.ContentTemplateSelector = Diagram.NodeTemplateSelector;
                node.DataContext = item;
            }

            //if (element is Connector connector)
            //{
            //}
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            if (element is Node node)
            {
                node.ContentTemplateSelector = null;
                node.Visual = null;
                node.Geometry = null;
                node.Content = null;
                node.DataContext = null;
            }
        }

        #endregion

        #region Event handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _graphNodesTracker.Activate();
            _selectedItemsTracker.Activate();
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            //Handles after Alt + Tab as well
            _dragOperation?.Rollback();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject originalSource && originalSource.FindVisualParentOf<Node>() is Node node)
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    if (!Diagram.SelectedObjects.Contains(node.DataContext))
                    {
                        Diagram.SelectedObjects.Add(node.DataContext);
                    }
                }
                else
                {
                    if (!Diagram.SelectedObjects.Contains(node.DataContext))
                    {
                        var obsoleteSelection = Diagram.SelectedObjects.Except(new[] { node.DataContext }).ToArray();
                        obsoleteSelection.ForEach(o => Diagram.SelectedObjects.Remove(o));
                        if (!Diagram.SelectedObjects.Any()) Diagram.SelectedObjects.Add(node.DataContext);
                    }
                }
            }
            else
            {
                Diagram.SelectedObjects.Clear();
            }

            if (Diagram.SelectedObjects.Any())
            {
                var selectedContainers = Diagram.SelectedObjects.Select(o => (UIElement)ItemContainerGenerator.ContainerFromItem(o));
                _dragOperation = new ItemsDragOperation(this, e.GetPosition(this), selectedContainers);
            }
            else
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    _dragOperation = new ViewBoxOffsetDragOperation(this, e.GetPosition(this), this);
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _dragOperation?.Update(e.GetPosition(this));
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragOperation?.Commit();
        }

        private void OnSelectedObjectsAttachment(object sender, ObjectAttachmentEventArgs e)
        {
            if (e.Object is INode node)
            {
                if (e is ObjectAttachedEventArgs) node.Visual.IsSelected = true;
                if (e is ObjectDetachedEventArgs) node.Visual.IsSelected = false;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _graphNodesTracker.Deactivate();
            _selectedItemsTracker.Deactivate();
        }

        private void OnVertexAttachment(object sender, ObjectAttachmentEventArgs e)
        {
            if (e.Route.Match(QueryNodeRoute) && e.Object is INode node)
            {
                if (e is ObjectAttachedEventArgs)
                {
                    if (node.Visual.IsSelected) Diagram.SelectedObjects.AddUnique(node);
                }

                if (e is ObjectDetachedEventArgs)
                {
                    Diagram.SelectedObjects.Remove(node);
                }
            }
        }

        private void OnVertexPropertyChanged(object sender, ChangedPropertyEventArgs e)
        {
            var tracker = (DeepTracker)sender;

            if (e.Route.Match(QueryNodeVisualSelectionRoute))
            {
                var node = (INode)tracker.GetObject(e.Route.Select(QueryNodeRoute));
                if (e.NewValue.AreEqual(true))
                {
                    Diagram.SelectedObjects.AddUnique(node);
                }
                else
                {
                    Diagram.SelectedObjects.Remove(node);
                }
            }
            else if (e.Route.Match(QueryNodeVisualGeometryRoute))
            {
                //TODO: disable handling on drag operations
                var node = (INode)tracker.GetObject(e.Route.Select(QueryNodeRoute));
                var container = (UIElement)ItemContainerGenerator.ContainerFromItem(node);
                if (e.Route.EndWith(Route.Create(nameof(INodeGeometry.CenterX))))
                {
                    var delta = (double)e.OldValue - (double)e.NewValue;
                    Canvas.SetLeft(container, Canvas.GetLeft(container) - delta);
                }
                else if (e.Route.EndWith(Route.Create(nameof(INodeGeometry.CenterY))))
                {
                    var delta = (double)e.OldValue - (double)e.NewValue;
                    Canvas.SetTop(container, Canvas.GetTop(container) - delta);
                }
            }

            Debug.WriteLine(e.FormatMessage());
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.Shell.Module.Diagram;component/Controls/DiagramWorkspace.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default DiagramWorkspace style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default DiagramWorkspace control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}