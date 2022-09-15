using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependencyRegisterAsInterfaceAttribute : DependencyRegisterAttribute
    {
        public DependencyRegisterAsInterfaceAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType));
        }

        public Type InterfaceType { get; }
    }
}
