using System;
using System.Windows;
using System.Windows.Controls;

namespace PS.Shell.Module.Diagram.Controls
{
    public class DiagramCanvas : Canvas
    {
        #region Property definitions

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register(nameof(Offset),
                                        typeof(Point),
                                        typeof(DiagramCanvas),
                                        new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsArrange));

        #endregion

        #region Properties

        public Point Offset
        {
            get { return (Point)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        #endregion

        #region Override members

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var minLeft = 0.0;
            var minTop = 0.0;
            var maxRight = 0.0;
            var maxBottom = 0.0;

            foreach (UIElement internalChild in InternalChildren)
            {
                if (internalChild == null) continue;

                var x = 0.0;
                var y = 0.0;
                var left = GetLeft(internalChild);
                if (!double.IsNaN(left))
                {
                    x = left;
                }
                else
                {
                    var right = GetRight(internalChild);
                    if (!double.IsNaN(right))
                    {
                        x = arrangeSize.Width - internalChild.DesiredSize.Width - right;
                    }
                }

                var top = GetTop(internalChild);
                if (!double.IsNaN(top))
                {
                    y = top;
                }
                else
                {
                    var bottom = GetBottom(internalChild);
                    if (!double.IsNaN(bottom))
                    {
                        y = arrangeSize.Height - internalChild.DesiredSize.Height - bottom;
                    }
                }

                minLeft = Math.Min(minLeft, x);
                minTop = Math.Min(minTop, y);
                maxRight = Math.Max(maxRight, x + internalChild.DesiredSize.Width);
                maxBottom = Math.Max(maxBottom, y + internalChild.DesiredSize.Height);
                internalChild.Arrange(new Rect(new Point(x + Offset.X, y + Offset.Y), internalChild.DesiredSize));
            }

            return arrangeSize;
        }

        #endregion
    }
}