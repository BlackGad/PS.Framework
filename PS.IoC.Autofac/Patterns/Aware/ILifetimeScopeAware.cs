using Autofac;

namespace PS.IoC.Patterns.Aware
{
    public interface ILifetimeScopeAware
    {
        #region Properties

        ILifetimeScope Scope { get; }

        #endregion
    }
}