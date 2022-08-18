using System;
using System.Collections.Specialized;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class ChangedCollectionEventArgs : ChangedEventArgs
    {
        public ChangedCollectionEventArgs(Route route, object collection, NotifyCollectionChangedEventArgs eventArgs)
            : base(route)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            EventArgs = eventArgs ?? throw new ArgumentNullException(nameof(eventArgs));
        }

        public object Collection { get; }

        public NotifyCollectionChangedEventArgs EventArgs { get; }
    }
}
