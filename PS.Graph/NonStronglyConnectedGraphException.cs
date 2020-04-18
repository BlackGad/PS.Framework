using System;

namespace PS.Graph
{
    /// <summary>
    ///     Exception raised when an algorithm detects a non-strongly connected graph.
    /// </summary>
    [Serializable]
    public class NonStronglyConnectedGraphException : QuickGraphException
    {
        #region Constructors

        public NonStronglyConnectedGraphException()
        {
        }

        public NonStronglyConnectedGraphException(string message)
            : base(message)
        {
        }

        public NonStronglyConnectedGraphException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NonStronglyConnectedGraphException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}