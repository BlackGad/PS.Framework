using System.Windows;
using System.Windows.Media;

// ReSharper disable UnusedMember.Global

namespace PS.WPF.Components
{
    public static class StoreBrush
    {
        #region Property definitions

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.RegisterAttached("Accent",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached("BorderBrush",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ContextualBackgroundProperty =
            DependencyProperty.RegisterAttached("ContextualBackground",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty FocusedBorderProperty =
            DependencyProperty.RegisterAttached("FocusedBorder",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty FocusedProperty =
            DependencyProperty.RegisterAttached("Focused",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty MouseOverBorderProperty =
            DependencyProperty.RegisterAttached("MouseOverBorder",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty MouseOverProperty =
            DependencyProperty.RegisterAttached("MouseOver",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PressedBorderProperty =
            DependencyProperty.RegisterAttached("PressedBorder",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PressedProperty =
            DependencyProperty.RegisterAttached("Pressed",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty SelectedBorderProperty =
            DependencyProperty.RegisterAttached("SelectedBorder",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty SelectedForegroundProperty =
            DependencyProperty.RegisterAttached("SelectedForeground",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.RegisterAttached("Selected",
                                                typeof(Brush),
                                                typeof(StoreBrush),
                                                new PropertyMetadata(default(Brush)));

        #endregion

        #region Static members

        public static Brush GetAccent(DependencyObject element)
        {
            return (Brush)element.GetValue(AccentProperty);
        }

        public static Brush GetBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(BackgroundProperty);
        }

        public static Brush GetBorderBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(BorderBrushProperty);
        }

        public static Brush GetContextualBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(ContextualBackgroundProperty);
        }

        public static Brush GetFocused(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusedProperty);
        }

        public static Brush GetFocusedBorder(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusedBorderProperty);
        }

        public static Brush GetForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(ForegroundProperty);
        }

        public static Brush GetMouseOver(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverProperty);
        }

        public static Brush GetMouseOverBorder(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverBorderProperty);
        }

        public static Brush GetPressed(DependencyObject element)
        {
            return (Brush)element.GetValue(PressedProperty);
        }

        public static Brush GetPressedBorder(DependencyObject element)
        {
            return (Brush)element.GetValue(PressedBorderProperty);
        }

        public static Brush GetSelected(DependencyObject element)
        {
            return (Brush)element.GetValue(SelectedProperty);
        }

        public static Brush GetSelectedBorder(DependencyObject element)
        {
            return (Brush)element.GetValue(SelectedBorderProperty);
        }

        public static Brush GetSelectedForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(SelectedForegroundProperty);
        }

        public static void SetAccent(DependencyObject element, Brush value)
        {
            element.SetValue(AccentProperty, value);
        }

        public static void SetBackground(DependencyObject element, Brush value)
        {
            element.SetValue(BackgroundProperty, value);
        }

        public static void SetBorderBrush(DependencyObject element, Brush value)
        {
            element.SetValue(BorderBrushProperty, value);
        }

        public static void SetContextualBackground(DependencyObject element, Brush value)
        {
            element.SetValue(ContextualBackgroundProperty, value);
        }

        public static void SetFocused(DependencyObject element, Brush value)
        {
            element.SetValue(FocusedProperty, value);
        }

        public static void SetFocusedBorder(DependencyObject element, Brush value)
        {
            element.SetValue(FocusedBorderProperty, value);
        }

        public static void SetForeground(DependencyObject element, Brush value)
        {
            element.SetValue(ForegroundProperty, value);
        }

        public static void SetMouseOver(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverProperty, value);
        }

        public static void SetMouseOverBorder(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverBorderProperty, value);
        }

        public static void SetPressed(DependencyObject element, Brush value)
        {
            element.SetValue(PressedProperty, value);
        }

        public static void SetPressedBorder(DependencyObject element, Brush value)
        {
            element.SetValue(PressedBorderProperty, value);
        }

        public static void SetSelected(DependencyObject element, Brush value)
        {
            element.SetValue(SelectedProperty, value);
        }

        public static void SetSelectedBorder(DependencyObject element, Brush value)
        {
            element.SetValue(SelectedBorderProperty, value);
        }

        public static void SetSelectedForeground(DependencyObject element, Brush value)
        {
            element.SetValue(SelectedForegroundProperty, value);
        }

        #endregion
    }
}