using System.Windows;

namespace PS.WPF.RoutedEvents
{
    public class ItemEventArgs<T> : RoutedEventArgs
    {
        #region Constructors

        public ItemEventArgs(RoutedEvent routedEvent, T item)
            : base(routedEvent)
        {
            Item = item;
        }

        public ItemEventArgs(RoutedEvent routedEvent, object source, T item)
            : base(routedEvent, source)
        {
            Item = item;
        }

        public ItemEventArgs(T item)
        {
            Item = item;
        }

        #endregion

        #region Properties

        public T Item { get; }

        #endregion
    }

    public class ItemEventArgs : ItemEventArgs<object>
    {
        #region Constructors

        public ItemEventArgs(RoutedEvent routedEvent, object item)
            : base(routedEvent, item)
        {
        }

        public ItemEventArgs(RoutedEvent routedEvent, object source, object item)
            : base(routedEvent, source, item)
        {
        }

        public ItemEventArgs(object item)
            : base(item)
        {
        }

        #endregion
    }
}