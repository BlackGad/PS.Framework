using Autofac;

namespace PS.IoC.Patterns.Aware
{
    public interface ISatisfyScopeAware
    {
        void Satisfy(ILifetimeScope scope);
    }
}
