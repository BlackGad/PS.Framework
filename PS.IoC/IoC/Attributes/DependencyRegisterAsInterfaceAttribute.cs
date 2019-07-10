using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependencyRegisterAsInterfaceAttribute : DependencyRegisterAttribute
    {
        #region Constructors

        public DependencyRegisterAsInterfaceAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType));
        }

        #endregion

        #region Properties

        public Type InterfaceType { get; }

        #endregion
    }
}