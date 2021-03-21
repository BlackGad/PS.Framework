using System.Collections;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class CollectionDetachedEventArgs : CollectionAttachmentEventArgs
    {
        public CollectionDetachedEventArgs(Route route, IEnumerable collection)
            : base(route, collection)
        {
        }
    }
}