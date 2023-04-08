using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PS.IoC.Bootstrapper
{
    public abstract class ServiceProviderBootstrapper : Bootstrapper<IServiceScope>
    {
        protected ServiceProviderBootstrapper(IBootstrapperLogger logger)
            : base(logger)
        {
        }

        protected sealed override IServiceScope CreateContainer(IBootstrapperLogger logger)
        {
            logger.Trace("Configuring IOC builder");

            var services = new ServiceCollection();
            RegisterContainerTypes(logger, services);
            DecorateServices(services);

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
            var serviceScope = services.BuildServiceProvider().CreateScope();

            logger.Trace("Post container creation actions execution");

            foreach (var action in containerCreateActions)
            {
                action.PostContainerCreateAction?.Invoke(serviceScope.ServiceProvider);
            }

            return serviceScope;
        }

        protected override void DisposeContainer(IBootstrapperLogger logger, IServiceScope container)
        {
            logger.Trace("Disposing IOC container");
            container.Dispose();
            logger.Debug("IOC container disposed");
        }

        protected abstract void RegisterContainerTypes(IBootstrapperLogger logger, IServiceCollection services);

        private void DecorateServices(ServiceCollection services)
        {
        }
    }
}
