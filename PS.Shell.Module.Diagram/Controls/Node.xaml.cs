using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using PS.Shell.Module.Diagram.Controls.MVVM;
using PS.WPF.Resources;

namespace PS.Shell.Module.Diagram.Controls
{
    public class Node : ContentControl
    {
        #region Property definitions

        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register(nameof(Geometry),
                                        typeof(INodeGeometry),
                                        typeof(Node),
                                        new FrameworkPropertyMetadata(default(INodeGeometry)));

        public static readonly DependencyProperty VisualProperty =
            DependencyProperty.Register(nameof(Visual),
                                        typeof(INodeVisual),
                                        typeof(Node),
                                        new FrameworkPropertyMetadata(default(INodeVisual)));

        #endregion

        #region Constructors

        static Node()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Node), new FrameworkPropertyMetadata(typeof(Node)));
            ResourceHelper.SetDefaultStyle(typeof(Node), Resource.ControlStyle);
        }

        public Node()
        {
            AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDownEvent));
            DataContextChanged += OnDataContextChanged;
        }

        #endregion

        #region Properties

        public INodeGeometry Geometry
        {
            get { return (INodeGeometry)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        public INodeVisual Visual
        {
            get { return (INodeVisual)GetValue(VisualProperty); }
            set { SetValue(VisualProperty, value); }
        }

        #endregion

        #region Event handlers

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BindingOperations.ClearAllBindings(this);
            if (e.NewValue is INode)
            {
                BindingOperations.SetBinding(this, ContentProperty, new Binding(nameof(INode.ViewModel)));
                BindingOperations.SetBinding(this, VisualProperty, new Binding(nameof(INode.Visual)));
                BindingOperations.SetBinding(this, GeometryProperty, new Binding(nameof(INode.Geometry)));
            }
        }

        private void OnPreviewMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.Shell.Module.Diagram;component/Controls/Node.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default Node style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default Node control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}