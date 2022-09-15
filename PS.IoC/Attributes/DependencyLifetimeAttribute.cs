using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyLifetimeAttribute : Attribute
    {
        public DependencyLifetimeAttribute(DependencyLifetime lifetime = DependencyLifetime.InstancePerDependency, params object[] scopes)
        {
            Scopes = scopes;
            Lifetime = lifetime;
        }

        public DependencyLifetime Lifetime { get; }

        public object[] Scopes { get; }
    }
}
