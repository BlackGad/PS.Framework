using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace PS.WPF.Markup
{
    public static class PasswordAttachedProperties
    {
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.RegisterAttached("Binding",
                                                typeof(string),
                                                typeof(PasswordAttachedProperties),
                                                new FrameworkPropertyMetadata("password",
                                                                              FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                              OnBindingPropertyChanged));

        private static readonly DependencyProperty ChangeHandlerProperty =
            DependencyProperty.RegisterAttached("ChangeHandler",
                                                typeof(RoutedEventHandler),
                                                typeof(PasswordAttachedProperties),
                                                new PropertyMetadata(default(RoutedEventHandler)));

        public static string GetBinding(DependencyObject dp)
        {
            return (string)dp.GetValue(BindingProperty);
        }

        public static MethodInfo SelectMethod { get; }

        public static void SetBinding(DependencyObject dp, string value)
        {
            dp.SetValue(BindingProperty, value);
        }

        private static void OnBindingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (passwordBox.GetValue(ChangeHandlerProperty) == null)
                {
                    var handler = new RoutedEventHandler(OnPasswordChanged);
                    passwordBox.AddHandler(PasswordBox.PasswordChangedEvent, handler);
                    passwordBox.SetValue(ChangeHandlerProperty, handler);
                }

                var newValue = passwordBox.GetValue(BindingProperty) as string ?? string.Empty;
                passwordBox.Password = newValue;
                SelectMethod.Invoke(passwordBox, new object[] { newValue.Length, newValue.Length });
            }
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.SetCurrentValue(BindingProperty, passwordBox.Password);
            }
        }

        static PasswordAttachedProperties()
        {
            SelectMethod = typeof(PasswordBox).GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
