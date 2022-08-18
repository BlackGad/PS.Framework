using System;
using System.Windows;

namespace PS.WPF.XAML
{
    public class XamlObjectProvider : Freezable
    {
        public static readonly DependencyProperty ObjectProperty =
            DependencyProperty.Register("Object", typeof(object), typeof(XamlObjectProvider), new UIPropertyMetadata(OnObjectPropertyChanged));

        private static void OnObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (XamlObjectProvider)d;
            owner.ObjectPropertyChanged?.Invoke(d, EventArgs.Empty);
        }

        public object Object
        {
            get { return GetValue(ObjectProperty); }
            set { SetValue(ObjectProperty, value); }
        }

        public event EventHandler<EventArgs> ObjectPropertyChanged;

        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }
    }

    public class XamlObjectProvider<T> : Freezable
    {
        public static readonly DependencyProperty ObjectProperty =
            DependencyProperty.Register("Object", typeof(T), typeof(XamlObjectProvider<T>), new UIPropertyMetadata(OnObjectPropertyChanged));

        private static void OnObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (XamlObjectProvider<T>)d;
            owner.ObjectPropertyChanged?.Invoke(d, EventArgs.Empty);
        }

        public T Object
        {
            get { return (T)GetValue(ObjectProperty); }
            set { SetValue(ObjectProperty, value); }
        }

        public event EventHandler<EventArgs> ObjectPropertyChanged;

        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }
    }
}
