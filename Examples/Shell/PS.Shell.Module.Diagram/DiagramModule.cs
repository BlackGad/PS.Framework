using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Module.Diagram.ViewModels;
using PS.Shell.Module.Diagram.Views;
using PS.WPF.DataTemplate;

namespace PS.Shell.Module.Diagram
{
    public class DiagramModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
        }

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.AssociateTemplate<EditorViewModel>(scope.Resolve<IDataTemplate<EditorView>>());
        }
    }
}
