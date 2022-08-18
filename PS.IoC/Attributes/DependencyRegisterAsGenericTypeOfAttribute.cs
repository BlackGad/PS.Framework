using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependencyRegisterAsGenericTypeOfAttribute : DependencyRegisterAttribute
    {
        public DependencyRegisterAsGenericTypeOfAttribute(Type genericType)
        {
            GenericType = genericType ?? throw new ArgumentNullException(nameof(genericType));
        }

        public Type GenericType { get; }
    }
}
