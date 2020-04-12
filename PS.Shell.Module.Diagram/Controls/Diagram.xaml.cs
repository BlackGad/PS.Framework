using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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

        #region Static members

        private static object OnSelectedObjectsCoerce(DependencyObject d, object baseValue)
        {
            return baseValue ?? new ObservableCollection<object>();
        }

        #endregion

        #region Constructors

        static Diagram()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Diagram), new FrameworkPropertyMetadata(typeof(Diagram)));
            ResourceHelper.SetDefaultStyle(typeof(Diagram), Resource.ControlStyle);
        }

        public Diagram()
        {
            CoerceValue(SelectedObjectsProperty);
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