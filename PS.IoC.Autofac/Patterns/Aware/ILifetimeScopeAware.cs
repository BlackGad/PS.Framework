using Autofac;

namespace PS.IoC.Patterns.Aware
{
    public interface ILifetimeScopeAware
    {
        ILifetimeScope Scope { get; }
    }
}
