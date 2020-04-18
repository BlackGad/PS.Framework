using System;

namespace PS.Graph
{
    [Serializable]
    public class NegativeWeightException : QuickGraphException
    {
        #region Constructors

        public NegativeWeightException()
        {
        }

        public NegativeWeightException(string message)
            : base(message)
        {
        }

        public NegativeWeightException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NegativeWeightException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}