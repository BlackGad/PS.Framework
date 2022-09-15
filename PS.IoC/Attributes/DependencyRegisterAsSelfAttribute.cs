using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DependencyRegisterAsSelfAttribute : DependencyRegisterAttribute
    {
    }
}
