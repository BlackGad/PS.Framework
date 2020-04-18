using System;

namespace PS.Graph
{
    [Serializable]
    public class NonAcyclicGraphException : QuickGraphException
    {
        #region Constructors

        public NonAcyclicGraphException()
        {
        }

        public NonAcyclicGraphException(string message)
            : base(message)
        {
        }

        public NonAcyclicGraphException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NonAcyclicGraphException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}