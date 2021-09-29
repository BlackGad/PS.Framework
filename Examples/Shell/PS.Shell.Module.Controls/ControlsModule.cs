using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Infrastructure.Models;
using PS.Shell.Module.Controls.ViewModels;
using PS.Shell.Module.Controls.Views;
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
            service.Controls.Add(scope.Resolve<TextBoxGenericViewModel>());
            service.Controls.Add(scope.Resolve<DecimalTextBoxGenericViewModel>());
        }

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.AssociateTemplate<TextBoxGenericViewModel>(scope.Resolve<IDataTemplate<TextBoxGenericView>>());
            service.AssociateTemplate<DecimalTextBoxGenericViewModel>(scope.Resolve<IDataTemplate<DecimalTextBoxGenericView>>());
        }

        #endregion
    }
}