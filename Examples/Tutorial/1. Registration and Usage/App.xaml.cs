using System.Windows;
using Example.ViewModels;
using Example.Views;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.WPF.DataTemplate;

namespace Example;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var viewResolverService = GetViewResolverService();

        // Register views in default region
        viewResolverService.AssociateTemplate<Item1ViewModel>(CreateDynamicDataTemplateForType<Item1View>());
        viewResolverService.AssociateTemplate<Item2ViewModel>(CreateDynamicDataTemplateForType<Item2View>());
        viewResolverService.AssociateTemplate<Item3ViewModel>((DataTemplate)Resources["Item3ViewDataTemplate"]);

        base.OnStartup(e);
    }

    private DataTemplate CreateDynamicDataTemplateForType<T>()
    {
        // Demonstrates how to create DataTemplate for view model type using custom ViewDataTemplate from PS.WPF
        return new ViewDataTemplate
        {
            ViewType = typeof(T)
        };
    }

    private IViewResolverService GetViewResolverService()
    {
        var viewResolverService = new ViewResolverService();

        // Service registration using ServiceLocator pattern
        //GlobalServices.Register(viewResolverService);
        
        // Service registration using Application.Resources
        Resources[typeof(IViewResolverService)] = viewResolverService;

        return viewResolverService;
    }
}
