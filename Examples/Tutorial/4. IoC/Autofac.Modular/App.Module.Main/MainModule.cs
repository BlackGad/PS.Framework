using App.Infrastructure;
using App.Module.Main.ViewModels;
using App.Module.Main.Views;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.WPF.DataTemplate;

namespace App.Module.Main;

public class MainModule : Autofac.Module
{
    protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
    {
        registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
        registration.HandleActivation<IModelResolverService>(ModelResolverServiceActivation);
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
    }

    private void ModelResolverServiceActivation(ILifetimeScope scope, IModelResolverService service)
    {
        service.Object(Regions.SHELL_LAYOUT).Value = scope.Resolve<ShellViewModel>();

        service.Collection(Regions.SHELL_ITEMS).Add(scope.Resolve<Item1ViewModel>());
    }

    private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
    {
        service.AssociateTemplate<Item1ViewModel>(scope.Resolve<IDataTemplate<Item1View>>());

        service.Associate<ShellViewModel>(
            template: scope.Resolve<IDataTemplate<ShellView>>(),
            style: XamlResources.MainWindowStyle);

        service.Region(Regions.SHELL_ITEMS_VIEW)
               .AssociateTemplate<object>(scope.Resolve<IDataTemplate<DefaultShellItemView>>());
    }
}
