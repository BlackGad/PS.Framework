using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Infrastructure.Models.ExamplesService;
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
            registration.HandleActivation<IExamplesService>(ControlsServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            var existingNames = ThisAssembly.GetManifestResourceNames();

            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
        }

        #endregion

        #region Members

        private void ControlsServiceActivation(ILifetimeScope scope, IExamplesService service)
        {
            service.Add<TextBoxViewModel>("Controls", "TextBox");
            service.Add<DecimalTextBoxViewModel>("Controls", "DecimalTextBox");
            service.Add<CancelableProcessCommandViewModel>("Commands", "CancelableProcessCommand");
            service.Add<ButtonsViewModel>("Controls", "Buttons");
            service.Add<BusyContainerSimpleViewModel>("Controls", "BusyContainer - simple");
            service.Add<BusyContainerAdvancedViewModel>("Controls", "BusyContainer - advanced");
            service.Add<BusyContainerStackViewModel>("Controls", "BusyContainer - stack");
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