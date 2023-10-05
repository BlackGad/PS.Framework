using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PS.IoC.Bootstrapper
{
    public abstract class ServiceProviderBootstrapper : Bootstrapper<IServiceProvider>
    {
        protected ServiceProviderBootstrapper(IBootstrapperLogger logger)
            : base(logger)
        {
        }

        protected sealed override IServiceProvider CreateContainer(IBootstrapperLogger logger)
        {
            logger.Trace("Configuring IOC builder");

            var services = new ServiceCollection();
            RegisterContainerTypes(logger, services);

            var containerCreateActions = services
                                         .Where(d => d.ServiceType == typeof(ContainerCreateActions))
                                         .Select(d => d.ImplementationInstance)
                                         .OfType<ContainerCreateActions>()
                                         .ToList();

            foreach (var action in containerCreateActions)
            {
                action.PreContainerCreateAction?.Invoke(services);
            }

            logger.Trace("Building IOC container");
            var serviceProvider = services.BuildServiceProvider();

            logger.Trace("Post container creation actions execution");

            foreach (var action in containerCreateActions)
            {
                action.PostContainerCreateAction?.Invoke(serviceProvider);
            }

            return serviceProvider;
        }

        protected override void DisposeContainer(IBootstrapperLogger logger, IServiceProvider container)
        {
            logger.Trace("Disposing IOC container");
            if (container is IDisposable disposable)
            {
                disposable.Dispose();
            }

            logger.Debug("IOC container disposed");
        }

        protected abstract void RegisterContainerTypes(IBootstrapperLogger logger, IServiceCollection services);
    }
}
