namespace PS.IoC.Attributes
{
    public enum DependencyLifetime
    {
        InstanceSingle,
        InstancePerDependency,
        InstancePerLifetimeScope,
        InstancePerMatchingLifetimeScope,
        InstancePerRequest
    }
}