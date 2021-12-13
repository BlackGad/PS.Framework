using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Infrastructure.Models.ExamplesService;
using PS.Shell.Module.NativeControls.ViewModels;
using PS.Shell.Module.NativeControls.Views;
using PS.WPF.DataTemplate;

namespace PS.Shell.Module.NativeControls
{
    public class NativeControlsModule : Autofac.Module
    {
        #region Override members

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
            registration.HandleActivation<IExamplesService>(ControlsServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
        }

        #endregion

        #region Members

        private void ControlsServiceActivation(ILifetimeScope scope, IExamplesService service)
        {
            service.Add<ButtonViewModel>("Native Controls", "Button")
                   .Source<ButtonViewModel>(@"ViewModels")
                   .XamlPage<ButtonView>(@"Views");
        }

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.AssociateTemplate<ButtonViewModel>(scope.Resolve<IDataTemplate<ButtonView>>());
        }

        #endregion
    }
}