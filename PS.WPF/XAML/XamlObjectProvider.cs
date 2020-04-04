using System;
using System.Windows;

namespace PS.WPF.XAML
{
    public class XamlObjectProvider : Freezable
    {
        #region Property definitions

        public static readonly DependencyProperty ObjectProperty =
            DependencyProperty.Register("Object", typeof(object), typeof(XamlObjectProvider), new UIPropertyMetadata(OnObjectPropertyChanged));

        #endregion

        #region Static members

        private static void OnObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (XamlObjectProvider)d;
            owner.ObjectPropertyChanged?.Invoke(d, new EventArgs());
        }

        #endregion

        #region Properties

        public object Object
        {
            get { return GetValue(ObjectProperty); }
            set { SetValue(ObjectProperty, value); }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> ObjectPropertyChanged;

        #endregion

        #region Override members

        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class XamlObjectProvider<T> : Freezable
    {
        #region Property definitions

        public static readonly DependencyProperty ObjectProperty =
            DependencyProperty.Register("Object", typeof(T), typeof(XamlObjectProvider<T>), new UIPropertyMetadata(OnObjectPropertyChanged));

        #endregion

        #region Static members

        private static void OnObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (XamlObjectProvider<T>)d;
            owner.ObjectPropertyChanged?.Invoke(d, new EventArgs());
        }

        #endregion

        #region Properties

        public T Object
        {
            get { return (T)GetValue(ObjectProperty); }
            set { SetValue(ObjectProperty, value); }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> ObjectPropertyChanged;

        #endregion

        #region Override members

        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}