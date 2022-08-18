using System;
using System.Windows;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Media;
using PS.Extensions;

namespace PS.WPF.Controls.Ribbon.Primitives
{
    public class RibbonContextualTabGroupsPanel : System.Windows.Controls.Ribbon.Primitives.RibbonContextualTabGroupsPanel
    {
        public static readonly DependencyProperty DesiredExtraPaddingProperty =
            DependencyProperty.RegisterAttached("DesiredExtraPadding",
                                                typeof(double),
                                                typeof(RibbonContextualTabGroupsPanel),
                                                new PropertyMetadata(default(double)));

        public static double GetDesiredExtraPadding(DependencyObject element)
        {
            return (double)element.GetValue(DesiredExtraPaddingProperty);
        }

        public static void SetDesiredExtraPadding(DependencyObject element, double value)
        {
            element.SetValue(DesiredExtraPaddingProperty, value);
        }

        /// <summary>
        /// To fix infinity measure loop we must override MeasureOverride
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = new Size();

            // Don't measure the child if tabs are not ready yet or Ribbon is collapsed.
            if (Ribbon == null || Ribbon.IsCollapsed) return desiredSize;

            var remainingSpace = availableSize.Width;
            var invalidateThPanel = false;
            RibbonTabHeadersPanel tabHeadersPanel = null;

            if (Ribbon.InternalPropertyGet("RibbonTabHeaderItemsControl") is RibbonTabHeaderItemsControl ribbonTabHeaderItemsControl)
            {
                tabHeadersPanel = ribbonTabHeaderItemsControl.InternalPropertyGet("InternalItemsHost") as RibbonTabHeadersPanel;
            }

            var tabHeadersPanelSpaceAvailable = (double?)tabHeadersPanel?.InternalPropertyGet("SpaceAvailable") ?? 0.0;

            foreach (RibbonContextualTabGroup tabGroupHeader in InternalChildren)
            {
                double width = 0;

                tabGroupHeader.InternalPropertySet("ArrangeWidth", 0);
                tabGroupHeader.InternalPropertySet("ArrangeX", 0);
                tabGroupHeader.InternalPropertySet("IdealDesiredWidth", 0.0);

                if (tabGroupHeader.Visibility == Visibility.Visible &&
                    tabGroupHeader.InternalPropertyGet("FirstVisibleTab") != null &&
                    remainingSpace > double.Epsilon)
                {
                    // Measure the maximum desired width 
                    // TabHeaders should be padded up more if needed. 
                    // Also we need to determine if we need to show the label tooltip
                    tabGroupHeader.Measure(new Size(double.PositiveInfinity, availableSize.Height));
                    tabGroupHeader.InternalPropertySet("IdealDesiredWidth", tabGroupHeader.DesiredSize.Width);

                    // If TabHeadersPanel has space to expand, then invalidate it so that TabHeaders add extra Padding to themselves. 
                    var idealDesiredWidth = (double)tabGroupHeader.InternalPropertyGet("IdealDesiredWidth");
                    var tabsDesiredWidth = (double)tabGroupHeader.InternalPropertyGet("TabsDesiredWidth");
                    var desiredExtraPadding = idealDesiredWidth - tabsDesiredWidth;
                    var desiredExtraPaddingFromPreviousMeasure = GetDesiredExtraPadding(tabGroupHeader);
                    // Bug fix for infinity measure loop when tabs group panel could not extend its size.
                    if (Math.Abs(desiredExtraPaddingFromPreviousMeasure - desiredExtraPadding) > double.Epsilon &&
                        desiredExtraPadding > double.Epsilon &&
                        tabHeadersPanelSpaceAvailable > double.Epsilon)
                    {
                        //Remember desiredExtraPadding for current tabGroupHeader
                        SetDesiredExtraPadding(tabGroupHeader, desiredExtraPadding);
                        invalidateThPanel = true;
                    }
                    else
                    {
                        //Clear desiredExtraPadding for current tabGroupHeader
                        SetDesiredExtraPadding(tabGroupHeader, 0);
                    }

                    width = tabsDesiredWidth;

                    // If the difference between tabGroupHeader.TabsDesiredWidth and remainingSpace is less
                    // than 1e-10 then assume that both are same. This is because TextBlock is very sensitive to 
                    // even a minute floating point difference and displays ellipsis even when sufficient
                    // space is available. 
                    if (Math.Abs(tabsDesiredWidth - remainingSpace) > double.Epsilon)
                    {
                        // Clip on the  left side
                        width = Math.Min(tabsDesiredWidth, remainingSpace);
                    }

                    tabGroupHeader.InternalPropertySet("ArrangeWidth", width);
                    tabGroupHeader.Measure(new Size(width, availableSize.Height));

                    // If label is truncated - show the tooltip
                    idealDesiredWidth = (double)tabGroupHeader.InternalPropertyGet("IdealDesiredWidth");
                    tabGroupHeader.InternalPropertySet("ShowLabelToolTip", idealDesiredWidth > width);

                    remainingSpace = remainingSpace - width;
                }

                desiredSize.Width += width;
                desiredSize.Height = Math.Max(desiredSize.Height, tabGroupHeader.DesiredSize.Height);
            }

            var waitingForMeasure = (bool)this.InternalPropertyGet("WaitingForMeasure");
            if (waitingForMeasure || invalidateThPanel)
            {
                tabHeadersPanel?.InvalidateMeasure();
            }

            return desiredSize;
        }

        /// <summary>
        /// To hide contextual tab splitters we must override OnRender
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var background = Background;
            if (background == null) return;

            var renderSize = RenderSize;
            drawingContext.DrawRectangle(background, null, new Rect(0.0, 0.0, renderSize.Width, renderSize.Height));
        }
    }
}
