using System.Windows;

namespace PS.WPF.Components
{
    public static class StoreVarious
    {
        public static readonly DependencyProperty IsInvalidProperty =
            DependencyProperty.RegisterAttached("IsInvalid",
                                                typeof(bool),
                                                typeof(StoreVarious),
                                                new PropertyMetadata(OnIsInvalidChanged));

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.RegisterAttached("IsValid",
                                                typeof(bool),
                                                typeof(StoreVarious),
                                                new PropertyMetadata(OnIsValidChanged));

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached("Name",
                                                typeof(string),
                                                typeof(StoreVarious),
                                                new PropertyMetadata(default(string)));

        public static bool GetIsInvalid(DependencyObject element)
        {
            return (bool)element.GetValue(IsInvalidProperty);
        }

        public static bool GetIsValid(DependencyObject element)
        {
            return (bool)element.GetValue(IsValidProperty);
        }

        public static string GetName(DependencyObject element)
        {
            return (string)element.GetValue(NameProperty);
        }

        public static void SetIsInvalid(DependencyObject element, bool value)
        {
            element.SetValue(IsInvalidProperty, value);
        }

        public static void SetIsValid(DependencyObject element, bool value)
        {
            element.SetValue(IsValidProperty, value);
        }

        public static void SetName(DependencyObject element, string value)
        {
            element.SetValue(NameProperty, value);
        }

        private static void OnIsInvalidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newState = (bool)e.NewValue;
            SetIsValid(d, !newState);
        }

        private static void OnIsValidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newState = (bool)e.NewValue;
            SetIsInvalid(d, !newState);
        }
    }
}
