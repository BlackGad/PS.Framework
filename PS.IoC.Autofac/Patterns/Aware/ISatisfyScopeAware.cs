using Autofac;

namespace PS.IoC.Patterns.Aware
{
    public interface ISatisfyScopeAware
    {
        #region Members

        void Satisfy(ILifetimeScope scope);

        #endregion
    }
}