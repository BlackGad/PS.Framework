using System;
using Autofac;

namespace PS.IoC.Extensions
{
    public static class LifetimeScopeExtensions
    {
        #region Static members

        public static ILifetimeScope BeginDisposableLifetimeScope(this ILifetimeScope scope, Action<ContainerBuilder> configurationAction = null)
        {
            var result = scope.BeginLifetimeScope(configurationAction ?? (builder => { }));
            scope.Disposer.AddInstanceForDisposal(result);
            return result;
        }

        #endregion
    }
}