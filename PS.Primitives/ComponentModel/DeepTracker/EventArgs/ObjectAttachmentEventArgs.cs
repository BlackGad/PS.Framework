using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker
{
    public abstract class ObjectAttachmentEventArgs : AttachmentEventArgs
    {
        #region Constructors

        protected ObjectAttachmentEventArgs(Route route, object @object)
            : base(route)
        {
            Object = @object;
        }

        #endregion

        #region Properties

        public object Object { get; }

        #endregion
    }
}