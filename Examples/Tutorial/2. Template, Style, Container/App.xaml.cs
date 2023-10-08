using System.Windows;
using System.Windows.Controls;
using Example.ViewModels;
using PS.MVVM.Extensions;
using PS.MVVM.Services;

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
        viewResolverService.AssociateTemplate<Item1ViewModel>((DataTemplate)Resources["Item1DataTemplate"]);
        viewResolverService.AssociateStyle<Item2ViewModel>((Style)Resources["Item2Style"]);
        viewResolverService.AssociateContainer<Item3ViewModel>((ItemContainerTemplate)Resources["Item3ContainerTemplate"]);

        base.OnStartup(e);
    }

    private IViewResolverService GetViewResolverService()
    {
        var viewResolverService = new ViewResolverService();
        Resources[typeof(IViewResolverService)] = viewResolverService;
        return viewResolverService;
    }
}
