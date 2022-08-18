using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Infrastructure.Models.ExamplesService;
using PS.Shell.Module.Ribbon.ViewModels;
using PS.Shell.Module.Ribbon.Views;
using PS.WPF.DataTemplate;

namespace PS.Shell.Module.Ribbon
{
    public class RibbonModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
            registration.HandleActivation<IExamplesService>(ControlsServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
        }

        private void ControlsServiceActivation(ILifetimeScope scope, IExamplesService service)
        {
            service.Add<RibbonViewModel>("Ribbon", "XAML usage");
        }

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.AssociateTemplate<RibbonViewModel>(scope.Resolve<IDataTemplate<RibbonView>>());
        }
    }
}
