using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using PS.Patterns.Aware;

namespace PS.WPF.Docking.Controls
{
    [ContentProperty(nameof(Content))]
    public class DockingArea : HeaderedContentControl,
                               IIDAware<string>
    {
        #region Property definitions

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register(nameof(Id),
                                        typeof(string),
                                        typeof(DockingArea),
                                        new FrameworkPropertyMetadata(default(object)));

        #endregion

        private Point? _dragStartPoint;

        #region Constructors

        public DockingArea()
        {
            AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseDownEventHandler));
            AddHandler(MouseMoveEvent, new MouseEventHandler(MouseMoveEventHandler));
        }

        #endregion

        #region Override members

        protected override Size MeasureOverride(Size constraint)
        {
            var snapshot = DockingManager.GetDragOperationAreaLayoutSnapshot(this);
            return snapshot == null
                ? base.MeasureOverride(constraint)
                : new Size(snapshot.ActualWidth, snapshot.ActualHeight);
        }

        #endregion

        #region IIDAware<string> Members

        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        #endregion

        #region Event handlers

        private void MouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _dragStartPoint = e.GetPosition(this);
            }

            if (e.LeftButton == MouseButtonState.Released)
            {
                _dragStartPoint = null;
            }
        }

        private void MouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if (!_dragStartPoint.HasValue)
            {
                return;
            }

            var offset = _dragStartPoint.Value - e.GetPosition(this);
            if (offset.Length < 3)
            {
                return;
            }

            DockingManager.StartDraggingOperation(this);
        }

        #endregion
    }
}