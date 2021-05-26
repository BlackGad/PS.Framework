using System;
using System.Collections;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class CollectionAttachmentEventArgs : AttachmentEventArgs
    {
        #region Constructors

        protected CollectionAttachmentEventArgs(Route route, IEnumerable collection)
            : base(route)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        #endregion

        #region Properties

        public IEnumerable Collection { get; }

        #endregion
    }
}