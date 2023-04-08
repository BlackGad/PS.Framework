using Autofac;
using PS.IoC;
using PS.MVVM.Services;
using PS.MVVM.Services.CommandService;
using PS.Shell.Module.Controls;
using PS.Shell.Module.Diagram;
using PS.Shell.Module.NativeControls;
using PS.Shell.Module.Ribbon;

namespace PS.Shell
{
    public class Bootstrapper : Bootstrapper<ILifetimeScope>
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
            logger.Debug("View resolver initialized successfully");

            logger.Trace("Initializing command service...");
            container.Resolve<ICommandService>();
            logger.Debug("Command service initialized successfully");
        }

        protected override ILifetimeScope CreateContainer(IBootstrapperLogger logger)
        {
            logger.Trace("Configuring IOC builder");
            var builder = new ContainerBuilder();

            logger.Trace("Registering modules...");
            builder.RegisterModule<MainModule>();
            builder.RegisterModule<DiagramModule>();
            builder.RegisterModule<ControlsModule>();
            builder.RegisterModule<NativeControlsModule>();
            builder.RegisterModule<RibbonModule>();
            logger.Debug("Modules registered");

            logger.Trace("Building IOC container");
            return builder.Build();
        }
    }
}
