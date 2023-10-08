using App.Module.Custom;
using App.Module.Main;
using Autofac;
using PS.IoC;
using PS.IoC.Extensions;
using PS.MVVM.Services;

namespace App.Shell;

internal class Bootstrapper : Bootstrapper<ILifetimeScope>
{
    public Bootstrapper(IBootstrapperLogger logger)
        : base(logger)
    {
    }

    protected override void DisposeContainer(IBootstrapperLogger logger, ILifetimeScope container)
    {
        logger.Trace("Disposing IOC container");
        container.Dispose();
        logger.Debug("IOC container disposed");
    }

    protected override void ComposeContainer(IBootstrapperLogger logger, ILifetimeScope container)
    {
        logger.Trace("Initializing view resolver...");
        GlobalServices.Register(container.Resolve<IViewResolverService>());
        GlobalServices.Register(container.Resolve<IModelResolverService>());
        logger.Debug("View resolver initialized successfully");
    }

    protected override ILifetimeScope CreateContainer(IBootstrapperLogger logger)
    {
        logger.Trace("Configuring IOC builder");
        var builder = new ContainerBuilder();

        // Register shell types
        builder.RegisterAssemblyTypesWithAttributes(typeof(Bootstrapper).Assembly);

        // Modules can be discovered automatically via configuration
        logger.Trace("Registering modules...");

        builder.RegisterModule<MainModule>();
        builder.RegisterModule<CustomModule>();

        logger.Debug("Modules registered");

        logger.Trace("Building IOC container");
        return builder.Build();
    }
}
