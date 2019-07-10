using System.Collections;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class CollectionDetachedEventArgs : CollectionAttachmentEventArgs
    {
        #region Constructors

        public CollectionDetachedEventArgs(Route route, IEnumerable collection)
            : base(route, collection)
        {
        }

        #endregion
    }
}