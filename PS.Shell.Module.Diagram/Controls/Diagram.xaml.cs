using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.Extensions;
using PS.Shell.Module.Diagram.Controls.MVVM;
using PS.WPF.Resources;

namespace PS.Shell.Module.Diagram.Controls
{
    public class Diagram : Control
    {
        #region Property definitions

        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register(nameof(Graph),
                                        typeof(IDiagramGraph),
                                        typeof(Diagram),
                                        new FrameworkPropertyMetadata(default(IDiagramGraph)));

        public static readonly DependencyProperty NodeTemplateSelectorProperty =
            DependencyProperty.Register(nameof(NodeTemplateSelector),
                                        typeof(DataTemplateSelector),
                                        typeof(Diagram),
                                        new FrameworkPropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty SelectedObjectsProperty =
            DependencyProperty.Register(nameof(SelectedObjects),
                                        typeof(ObservableCollection<object>),
                                        typeof(Diagram),
                                        new FrameworkPropertyMetadata(null, OnSelectedObjectsCoerce));

        #endregion

        #region Constants

        private static readonly Route QueryNodeRoute;

        private static readonly Route QueryNodeVisualSelectionRoute;

        #endregion

        #region Static members

        private static object OnSelectedObjectsCoerce(DependencyObject d, object baseValue)
        {
            return baseValue ?? new ObservableCollection<object>();
        }

        #endregion

        private readonly DeepTracker _graphNodesTracker;
        private readonly DeepTracker _selectedItemsTracker;

        #region Constructors

        static Diagram()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Diagram), new FrameworkPropertyMetadata(typeof(Diagram)));
            ResourceHelper.SetDefaultStyle(typeof(Diagram), Resource.ControlStyle);

            QueryNodeRoute = Route.Create(nameof(Graph),
                                          nameof(IDiagramGraph.Vertices),
                                          Routes.Wildcard);

            QueryNodeVisualSelectionRoute = Route.Create(QueryNodeRoute,
                                                         nameof(INode.Visual),
                                                         nameof(INodeVisual.IsSelected));
        }

        public Diagram()
        {
            CoerceValue(SelectedObjectsProperty);

            _selectedItemsTracker = DeepTracker.Setup(this, nameof(SelectedObjects), Routes.Wildcard)
                                               .Subscribe<ObjectAttachmentEventArgs>(OnSelectedObjectsAttachment)
                                               .Create();

            _graphNodesTracker = DeepTracker.Setup(this, nameof(Graph), nameof(Graph.Vertices), Routes.WildcardRecursive)
                                            .Exclude<INode>(node => node.ViewModel)
                                            .Subscribe<ObjectAttachmentEventArgs>(OnVertexAttachment)
                                            .Subscribe<ChangedPropertyEventArgs>(OnVertexPropertyChanged)
                                            .Create();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        #endregion

        #region Properties

        public IDiagramGraph Graph
        {
            get { return (IDiagramGraph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        public DataTemplateSelector NodeTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(NodeTemplateSelectorProperty); }
            set { SetValue(NodeTemplateSelectorProperty, value); }
        }

        public ObservableCollection<object> SelectedObjects
        {
            get { return (ObservableCollection<object>)GetValue(SelectedObjectsProperty); }
            set { SetValue(SelectedObjectsProperty, value); }
        }

        #endregion

        #region Event handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _graphNodesTracker.Activate();
            _selectedItemsTracker.Activate();
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
                    if (node.Visual.IsSelected) SelectedObjects.AddUnique(node);
                }

                if (e is ObjectDetachedEventArgs)
                {
                    SelectedObjects.Remove(node);
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
                    SelectedObjects.AddUnique(node);
                }
                else
                {
                    SelectedObjects.Remove(node);
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
                new Uri("/PS.Shell.Module.Diagram;component/Controls/Diagram.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default Diagram style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default Diagram control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}