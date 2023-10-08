using System;
using System.Windows;
using App.Infrastructure;
using Autofac;
using PS.IoC;
using PS.MVVM.Services;
using PS.MVVM.Services.WindowService;

namespace App.Shell;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class CustomApplication
{
    private readonly Bootstrapper _bootstrapper;

    public CustomApplication()
    {
        try
        {
            var bootstrapperLogger = new RelayBootstrapperLogger
            {
                TraceAction = message => Console.WriteLine($"Trace: {message}"),
                DebugAction = message => Console.WriteLine($"Debug: {message}"),
                InfoAction = message => Console.WriteLine($"Info: {message}"),
                WarnAction = message => Console.WriteLine($"Warn: {message}"),
                ErrorAction = message => Console.WriteLine($"Error: {message}"),
                FatalAction = message => Console.WriteLine($"Fatal: {message}")
            };

            _bootstrapper = new Bootstrapper(bootstrapperLogger);
        }
        catch (Exception e)
        {
            FatalShutdown(e, "Bootstrapper cannot be created");
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            InitializeBootstrapper();

            base.OnStartup(e);

            ShowShell();
        }
        catch (Exception exception)
        {
            FatalShutdown(exception, "Application failed");
        }
    }

    private void FatalShutdown(Exception e, string message)
    {
        Console.WriteLine($"Fatal: {message}. Details: {e.Message}");
        Shutdown(-1);
    }

    private void InitializeBootstrapper()
    {
        try
        {
            _bootstrapper.Initialize();
        }
        catch (Exception exception)
        {
            throw new ApplicationException("Bootstrapper initialization failed", exception);
        }
    }

    private void ShowShell()
    {
        try
        {
            Console.WriteLine("Showing shell...");
            var container = _bootstrapper.Container;
            var modelResolverService = container.Resolve<IModelResolverService>();
            var windowService = container.Resolve<IWindowService>();

            windowService.Show(modelResolverService.Object(Regions.SHELL_LAYOUT).Value);
            Console.WriteLine("Shell shown successfully");
        }
        catch (Exception exception)
        {
            throw new ApplicationException("Shell initialization failed", exception);
        }
    }
}
