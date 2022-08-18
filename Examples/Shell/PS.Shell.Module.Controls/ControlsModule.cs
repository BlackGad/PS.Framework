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
            service.Add<TextBoxViewModel>("Controls", "TextBox")
                   .Source<TextBoxViewModel>(@"ViewModels")
                   .XamlPage<TextBoxView>(@"Views");

            service.Add<DecimalTextBoxViewModel>("Controls", "DecimalTextBox")
                   .Source<DecimalTextBoxViewModel>(@"ViewModels")
                   .XamlPage<DecimalTextBoxView>(@"Views");

            service.Add<CancelableProcessCommandViewModel>("Commands", "CancelableProcessCommand")
                   .Source<CancelableProcessCommandViewModel>(@"ViewModels")
                   .XamlPage<CancelableProcessCommandView>(@"Views");

            service.Add<ButtonsViewModel>("Controls", "Buttons")
                   .Source<ButtonsViewModel>(@"ViewModels")
                   .XamlPage<ButtonsView>(@"Views");

            service.Add<BusyContainerSimpleViewModel>("Controls", "BusyContainer - simple")
                   .Source<BusyContainerSimpleViewModel>(@"ViewModels")
                   .XamlPage<BusyContainerSimpleView>(@"Views");

            service.Add<BusyContainerAdvancedViewModel>("Controls", "BusyContainer - advanced")
                   .Source<BusyContainerAdvancedViewModel>(@"ViewModels")
                   .Source<CustomState>()
                   .Source<StateWithMutableDescription>()
                   .Source<StateWithToStringOverride>()
                   .XamlPage<BusyContainerAdvancedView>(@"Views");

            service.Add<BusyContainerStackViewModel>("Controls", "BusyContainer - stack")
                   .Source<BusyContainerStackViewModel>(@"ViewModels")
                   .XamlPage<BusyContainerStackView>(@"Views");
        }

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.AssociateTemplate<TextBoxViewModel>(scope.Resolve<IDataTemplate<TextBoxView>>())
                   .AssociateTemplate<DecimalTextBoxViewModel>(scope.Resolve<IDataTemplate<DecimalTextBoxView>>())
                   .AssociateTemplate<ButtonsViewModel>(scope.Resolve<IDataTemplate<ButtonsView>>())
                   .AssociateTemplate<BusyContainerSimpleViewModel>(scope.Resolve<IDataTemplate<BusyContainerSimpleView>>())
                   .AssociateTemplate<BusyContainerAdvancedViewModel>(scope.Resolve<IDataTemplate<BusyContainerAdvancedView>>())
                   .AssociateTemplate<BusyContainerStackViewModel>(scope.Resolve<IDataTemplate<BusyContainerStackView>>())
                   .AssociateTemplate<CancelableProcessCommandViewModel>(scope.Resolve<IDataTemplate<CancelableProcessCommandView>>());
        }
    }
}
