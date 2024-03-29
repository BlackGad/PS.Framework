﻿using System.Windows;

namespace PS.WPF.Components
{
    public static class StoreGeometry
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius",
                                                typeof(CornerRadius),
                                                typeof(StoreGeometry),
                                                new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.RegisterAttached("Height",
                                                typeof(double),
                                                typeof(StoreGeometry),
                                                new PropertyMetadata(default(double)));

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached("Width",
                                                typeof(double),
                                                typeof(StoreGeometry),
                                                new PropertyMetadata(default(double)));

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        public static double GetHeight(DependencyObject element)
        {
            return (double)element.GetValue(HeightProperty);
        }

        public static double GetWidth(DependencyObject element)
        {
            return (double)element.GetValue(WidthProperty);
        }

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static void SetHeight(DependencyObject element, double value)
        {
            element.SetValue(HeightProperty, value);
        }

        public static void SetWidth(DependencyObject element, double value)
        {
            element.SetValue(WidthProperty, value);
        }
    }
}
