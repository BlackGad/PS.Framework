using App.Infrastructure;
using App.Module.Custom.ViewModels;
using App.Module.Custom.Views;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.WPF.DataTemplate;

namespace App.Module.Custom;

public class CustomModule : Autofac.Module
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
        service.Collection(Regions.SHELL_ITEMS).Add(scope.Resolve<Item2ViewModel>());
    }

    private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
    {
        service.AssociateTemplate<Item2ViewModel>(scope.Resolve<IDataTemplate<Item2View>>());

        service.Region(Regions.SHELL_ITEMS_VIEW)
               .AssociateTemplate<Item2ViewModel>(scope.Resolve<IDataTemplate<Item2ShellItemView>>());
    }
}
