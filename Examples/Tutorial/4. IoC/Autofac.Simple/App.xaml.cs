using System.Windows;
using Autofac;
using Example.ViewModels;
using Example.Views;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.MVVM.Services.WindowService;
using PS.WPF.DataTemplate;

namespace Example;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Populate registrations
        var builder = new ContainerBuilder();
        builder.RegisterAssemblyTypesWithAttributes(typeof(App).Assembly);

        // Create container
        var container = builder.Build();
        

        // Register views in default region
        var viewResolverService = container.Resolve<IViewResolverService>();
        // Register IViewResolverService in GlobalServices
        Resources[typeof(IViewResolverService)] = viewResolverService;

        viewResolverService.AssociateTemplate<Item1ViewModel>(container.Resolve<IDataTemplate<Item1View>>());
        viewResolverService.AssociateTemplate<Item2ViewModel>(container.Resolve<IDataTemplate<Item2View>>());

        // Register main window style and data template
        viewResolverService.AssociateTemplate<MainViewModel>(container.Resolve<IDataTemplate<MainView>>());
        viewResolverService.AssociateStyle<MainViewModel>((Style)Resources["MainWindowStyle"]);

        // Show main window
        var windowService = container.Resolve<IWindowService>();
        // 1. Create view model in IWindowService implementation.
        // 2. Create WPF Window instance
        // 3. Insert view into it.
        // 4. Show window
        windowService.Show<MainViewModel>();

        base.OnStartup(e);
    }
}
