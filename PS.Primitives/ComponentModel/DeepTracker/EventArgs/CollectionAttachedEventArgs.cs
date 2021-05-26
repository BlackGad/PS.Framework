using System.Collections;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public class CollectionAttachedEventArgs : CollectionAttachmentEventArgs
    {
        #region Constructors

        public CollectionAttachedEventArgs(Route route, IEnumerable collection)
            : base(route, collection)
        {
        }

        #endregion
    }
}