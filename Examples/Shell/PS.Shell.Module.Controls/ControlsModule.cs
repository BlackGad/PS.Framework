using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Infrastructure.Models.ControlsService;
using PS.Shell.Module.Controls.ViewModels;
using PS.Shell.Module.Controls.ViewModels.BusyContainer;
using PS.Shell.Module.Controls.Views;
using PS.Shell.Module.Controls.Views.BusyContainer;
using PS.WPF.DataTemplate;

namespace PS.Shell.Module.Controls
{
    public class ControlsModule : Autofac.Module
    {
        #region Override members

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
            registration.HandleActivation<IControlsService>(ControlsServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
        }

        #endregion

        #region Members

        private void ControlsServiceActivation(ILifetimeScope scope, IControlsService service)
        {
            service.Controls.Add(scope.Resolve<TextBoxViewModel>());
            service.Controls.Add(scope.Resolve<DecimalTextBoxViewModel>());
            service.Controls.Add(scope.Resolve<CancelableProcessCommandViewModel>());
            service.Controls.Add(scope.Resolve<ButtonsViewModel>());
            service.Controls.Add(scope.Resolve<BusyContainerSimpleViewModel>());
            service.Controls.Add(scope.Resolve<BusyContainerAdvancedViewModel>());
            service.Controls.Add(scope.Resolve<BusyContainerStackViewModel>());
        }

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.AssociateTemplate<TextBoxViewModel>(scope.Resolve<IDataTemplate<TextBoxView>>())
                   .AssociateTemplate<DecimalTextBoxViewModel>(scope.Resolve<IDataTemplate<DecimalTextBoxView>>())
                   .AssociateTemplate<ButtonsViewModel>(scope.Resolve<IDataTemplate<ButtonsViewView>>())
                   .AssociateTemplate<BusyContainerSimpleViewModel>(scope.Resolve<IDataTemplate<BusyContainerSimpleView>>())
                   .AssociateTemplate<BusyContainerAdvancedViewModel>(scope.Resolve<IDataTemplate<BusyContainerAdvancedView>>())
                   .AssociateTemplate<BusyContainerStackViewModel>(scope.Resolve<IDataTemplate<BusyContainerStackView>>())
                   .AssociateTemplate<CancelableProcessCommandViewModel>(scope.Resolve<IDataTemplate<CancelableProcessCommandView>>());
        }

        #endregion
    }
}