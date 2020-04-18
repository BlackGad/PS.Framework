using System;

namespace PS.Graph
{
    [Serializable]
    public class ParallelEdgeNotAllowedException : QuickGraphException
    {
        #region Constructors

        public ParallelEdgeNotAllowedException()
        {
        }

        public ParallelEdgeNotAllowedException(string message)
            : base(message)
        {
        }

        public ParallelEdgeNotAllowedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ParallelEdgeNotAllowedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}