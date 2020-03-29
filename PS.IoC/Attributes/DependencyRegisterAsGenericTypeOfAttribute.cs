using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependencyRegisterAsGenericTypeOfAttribute : DependencyRegisterAttribute
    {
        #region Constructors

        public DependencyRegisterAsGenericTypeOfAttribute(Type genericType)
        {
            GenericType = genericType ?? throw new ArgumentNullException(nameof(genericType));
        }

        #endregion

        #region Properties

        public Type GenericType { get; }

        #endregion
    }
}