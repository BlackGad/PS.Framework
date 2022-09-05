using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Components;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class ScrollViewer : System.Windows.Controls.ScrollViewer
    {
        public static readonly DependencyProperty HorizontalFlowDirectionProperty =
            DependencyProperty.Register(nameof(HorizontalFlowDirection),
                                        typeof(FlowDirection),
                                        typeof(ScrollViewer),
                                        new FrameworkPropertyMetadata(FlowDirection.LeftToRight, OnHorizontalFlowDirectionChanged));

        public static readonly DependencyProperty VerticalFlowDirectionProperty =
            DependencyProperty.Register(nameof(VerticalFlowDirection),
                                        typeof(VerticalFlowDirection),
                                        typeof(ScrollViewer),
                                        new FrameworkPropertyMetadata(VerticalFlowDirection.TopToBottom, OnVerticalFlowDirectionChanged));

        private static void OnHorizontalFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (ScrollViewer)d;
            owner.ApplyFlowDirection();
        }

        private static void OnVerticalFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (ScrollViewer)d;
            owner.ApplyFlowDirection();
        }

        static ScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(typeof(ScrollViewer)));
            ResourceHelper.SetDefaultStyle(typeof(ScrollViewer), Resource.ControlStyle);
        }

        public FlowDirection HorizontalFlowDirection
        {
            get { return (FlowDirection)GetValue(HorizontalFlowDirectionProperty); }
            set { SetValue(HorizontalFlowDirectionProperty, value); }
        }

        public VerticalFlowDirection VerticalFlowDirection
        {
            get { return (VerticalFlowDirection)GetValue(VerticalFlowDirectionProperty); }
            set { SetValue(VerticalFlowDirectionProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ApplyFlowDirection();
        }

        private void ApplyFlowDirection()
        {
            var gridColumn1 = GetTemplateChild("ColumnDefinitionOne");
            var gridColumn2 = GetTemplateChild("ColumnDefinitionTwo");
            var gridRow1 = GetTemplateChild("RowDefinitionOne");
            var gridRow2 = GetTemplateChild("RowDefinitionTwo");
            var verticalScrollBar = GetTemplateChild("PART_VerticalScrollBar");
            var horizontalScrollBar = GetTemplateChild("PART_HorizontalScrollBar");
            var scrollContentPresenter = GetTemplateChild("PART_ScrollContentPresenter");

            var elements = new[]
            {
                gridColumn1,
                gridColumn2,
                gridRow1,
                gridRow2,
                verticalScrollBar,
                horizontalScrollBar,
                scrollContentPresenter,
            };

            if (elements.Any(e => e is null))
            {
                return;
            }

            // ReSharper disable PossibleNullReferenceException
            if (HorizontalFlowDirection == FlowDirection.LeftToRight)
            {
                gridColumn1.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Star));
                gridColumn2.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Auto));

                scrollContentPresenter.SetValue(Grid.ColumnProperty, 0);
                verticalScrollBar.SetValue(Grid.ColumnProperty, 1);
            }
            else
            {
                gridColumn1.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Auto));
                gridColumn2.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Star));

                scrollContentPresenter.SetValue(Grid.ColumnProperty, 1);
                verticalScrollBar.SetValue(Grid.ColumnProperty, 0);
            }

            if (VerticalFlowDirection == VerticalFlowDirection.TopToBottom)
            {
                gridRow1.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Star));
                gridRow2.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Auto));

                scrollContentPresenter.SetValue(Grid.RowProperty, 0);
                horizontalScrollBar.SetValue(Grid.RowProperty, 1);
            }
            else
            {
                gridRow1.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Auto));
                gridRow2.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Star));

                scrollContentPresenter.SetValue(Grid.RowProperty, 1);
                horizontalScrollBar.SetValue(Grid.RowProperty, 0);
            }
            // ReSharper restore PossibleNullReferenceException
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default = new Uri("/PS.WPF;component/Controls/ScrollViewer.xaml", UriKind.RelativeOrAbsolute);
            public static readonly ResourceDescriptor ControlStyle = ResourceDescriptor.Create<Style>(Default);
        }

        #endregion
    }
}
