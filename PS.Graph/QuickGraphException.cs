using System;

namespace PS.Graph
{
    [Serializable]
    public abstract class QuickGraphException : Exception
    {
        #region Constructors

        protected QuickGraphException()
        {
        }

        protected QuickGraphException(string message)
            : base(message)
        {
        }

        protected QuickGraphException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected QuickGraphException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}