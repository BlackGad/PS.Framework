using System;

namespace PS.Graph
{
    [Serializable]
    public class NegativeCycleGraphException : QuickGraphException
    {
        #region Constructors

        public NegativeCycleGraphException()
        {
        }

        public NegativeCycleGraphException(string message)
            : base(message)
        {
        }

        public NegativeCycleGraphException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NegativeCycleGraphException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}