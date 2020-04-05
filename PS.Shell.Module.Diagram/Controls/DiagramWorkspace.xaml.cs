using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private readonly SelectorService _selectorService;

        private DragOperation _dragOperation;

        #region Constructors

        static DiagramWorkspace()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramWorkspace), new FrameworkPropertyMetadata(typeof(DiagramWorkspace)));
            ResourceHelper.SetDefaultStyle(typeof(DiagramWorkspace), Resource.ControlStyle);
        }

        public DiagramWorkspace()
        {
            AddHandler(MouseDownEvent, new MouseButtonEventHandler(OnMouseDown));
            AddHandler(MouseMoveEvent, new MouseEventHandler(OnMouseMove));
            AddHandler(MouseUpEvent, new MouseButtonEventHandler(OnMouseUp));
            AddHandler(LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture));

            _selectorService = new SelectorService();
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
                node.ContentTemplateSelector = Diagram.NodeTemplateSelector;
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

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        #endregion

        #region Event handlers

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
                    _selectorService.Add(node);
                }
                else
                {
                    _selectorService.Set(node);
                }
            }
            else
            {
                _selectorService.Clear();
            }

            if (_selectorService.SelectedItems.Any())
            {
                _dragOperation = new ItemsDragOperation(this, e.GetPosition(this), _selectorService.SelectedItems);
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