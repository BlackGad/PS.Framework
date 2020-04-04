using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PS.Extensions;
using PS.Shell.Module.Diagram.Controls.MVVM;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.Shell.Module.Diagram.Controls
{
    public class DiagramCanvas : ItemsControl
    {
        #region Property definitions

        public static readonly DependencyProperty NodeTemplateSelectorProperty =
            DependencyProperty.Register(nameof(NodeTemplateSelector),
                                        typeof(DataTemplateSelector),
                                        typeof(DiagramCanvas),
                                        new FrameworkPropertyMetadata(default(DataTemplateSelector)));

        #endregion

        private UIElement _capturedElement;
        private Point? _initialElementPosition;
        private Point? _initialMousePosition;

        #region Constructors

        static DiagramCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramCanvas), new FrameworkPropertyMetadata(typeof(DiagramCanvas)));
            ResourceHelper.SetDefaultStyle(typeof(DiagramCanvas), Resource.ControlStyle);
        }

        public DiagramCanvas()
        {
            AddHandler(MouseDownEvent, new MouseButtonEventHandler(OnMouseDown));
            AddHandler(MouseMoveEvent, new MouseEventHandler(OnMouseMove));
            AddHandler(MouseUpEvent, new MouseButtonEventHandler(OnMouseUp));
            AddHandler(LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture));
        }

        #endregion

        #region Properties

        public DataTemplateSelector NodeTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(NodeTemplateSelectorProperty); }
            set { SetValue(NodeTemplateSelectorProperty, value); }
        }

        #endregion

        #region Override members

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Node || item is Connector;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Node();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element is Node node)
            {
                node.ContentTemplateSelector = NodeTemplateSelector;
                if (item is INode nodeViewModel)
                {
                    node.Content = nodeViewModel.ViewModel;
                }
            }

            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        #endregion

        #region Event handlers

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            //After Alt + Tab
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject originalSource)
            {
                var node = originalSource.FindVisualParentOf<Node>();
                _capturedElement = node;
            }

            if (_capturedElement == null) return;

            Mouse.Capture(this);

            _initialMousePosition = e.GetPosition(this);
            _initialElementPosition = new Point(Canvas.GetLeft(_capturedElement), Canvas.GetTop(_capturedElement));
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured.AreDiffers(this)) return;
            if (_initialMousePosition == null) return;
            if (_initialElementPosition == null) return;
            if (_capturedElement == null) return;

            var position = e.GetPosition(this);
            var delta = _initialMousePosition.Value - position;

            var newLeft = _initialElementPosition.Value.X - delta.X;
            var newTop = _initialElementPosition.Value.Y - delta.Y;

            Canvas.SetTop(_capturedElement, newTop);
            Canvas.SetLeft(_capturedElement, newLeft);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured.AreDiffers(this)) return;

            Mouse.Capture(null);
            _capturedElement = null;
            _initialMousePosition = null;
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.Shell.Module.Diagram;component/Controls/DiagramCanvas.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default DiagramCanvas style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default DiagramCanvas control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}