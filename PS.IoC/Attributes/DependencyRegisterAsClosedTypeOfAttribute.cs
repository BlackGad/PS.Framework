using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependencyRegisterAsClosedTypeOfAttribute : DependencyRegisterAttribute
    {
        public DependencyRegisterAsClosedTypeOfAttribute(Type closedType)
        {
            ClosedType = closedType ?? throw new ArgumentNullException(nameof(closedType));
        }

        public Type ClosedType { get; }
    }
}
