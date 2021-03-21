using System;
using System.Collections;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class CollectionAttachmentEventArgs : AttachmentEventArgs
    {
        protected CollectionAttachmentEventArgs(Route route, IEnumerable collection)
            : base(route)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public IEnumerable Collection { get; }
    }
}