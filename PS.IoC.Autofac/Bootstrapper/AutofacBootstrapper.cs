using Autofac;

namespace PS.IoC.Bootstrapper
{
    public abstract class AutofacBootstrapper : Bootstrapper<ILifetimeScope>
    {
        protected AutofacBootstrapper(IBootstrapperLogger logger)
            : base(logger)
        {
        }

        protected sealed override ILifetimeScope CreateContainer(IBootstrapperLogger logger)
        {
            logger.Trace("Configuring IOC builder");
            var builder = new ContainerBuilder();
            RegisterContainerTypes(logger, builder);
            logger.Trace("Building IOC container");
            return builder.Build();
        }

        protected override void DisposeContainer(IBootstrapperLogger logger, ILifetimeScope container)
        {
            logger.Trace("Disposing IOC container");
            container.Dispose();
            logger.Debug("IOC container disposed");
        }

        protected abstract void RegisterContainerTypes(IBootstrapperLogger logger, ContainerBuilder builder);
    }
}
