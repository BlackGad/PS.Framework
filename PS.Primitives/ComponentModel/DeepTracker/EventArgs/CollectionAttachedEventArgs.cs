using System.Collections;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class CollectionAttachedEventArgs : CollectionAttachmentEventArgs
    {
        public CollectionAttachedEventArgs(Route route, IEnumerable collection)
            : base(route, collection)
        {
        }
    }
}
