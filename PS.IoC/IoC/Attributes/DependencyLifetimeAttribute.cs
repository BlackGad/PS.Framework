using System;

namespace PS.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyLifetimeAttribute : Attribute
    {
        #region Constructors

        public DependencyLifetimeAttribute(DependencyLifetime lifetime = DependencyLifetime.InstancePerDependency, params object[] scopes)
        {
            Scopes = scopes;
            Lifetime = lifetime;
        }

        #endregion

        #region Properties

        public DependencyLifetime Lifetime { get; }

        public object[] Scopes { get; }

        #endregion
    }
}