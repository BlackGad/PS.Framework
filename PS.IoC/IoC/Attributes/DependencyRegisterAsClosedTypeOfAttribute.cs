using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependencyRegisterAsClosedTypeOfAttribute : DependencyRegisterAttribute
    {
        #region Constructors

        public DependencyRegisterAsClosedTypeOfAttribute(Type closedType)
        {
            ClosedType = closedType ?? throw new ArgumentNullException(nameof(closedType));
        }

        #endregion

        #region Properties

        public Type ClosedType { get; }

        #endregion
    }
}