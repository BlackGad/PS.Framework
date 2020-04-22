using System;

namespace PS.Graph
{
    [Serializable]
    public class ClusteredGraphNotSupportFunctionalityException : QuickGraphException
    {
        #region Constructors

        public ClusteredGraphNotSupportFunctionalityException(Type interfaceType)
            : this($"Clustered graph does not support functionality of {interfaceType.Name}", null)
        {
        }

        public ClusteredGraphNotSupportFunctionalityException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ClusteredGraphNotSupportFunctionalityException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}