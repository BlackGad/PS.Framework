using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PS.Patterns.Aware;
using PS.WPF.Resources;

namespace PS.Shell.Module.Diagram.Controls
{
    public class Node : ContentControl,
                        IIsSelectedAware
    {
        #region Property definitions

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected),
                                        typeof(bool),
                                        typeof(Node),
                                        new FrameworkPropertyMetadata());

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
        }

        #endregion

        #region IIsSelectedAware Members

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        #endregion

        #region Event handlers

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