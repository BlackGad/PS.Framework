using System;
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

        #endregion

        #region Static members

     
        #endregion

        #region Constructors

        static Diagram()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Diagram), new FrameworkPropertyMetadata(typeof(Diagram)));
            ResourceHelper.SetDefaultStyle(typeof(Diagram), Resource.ControlStyle);
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