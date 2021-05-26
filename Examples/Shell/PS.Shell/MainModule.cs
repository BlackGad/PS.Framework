using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Infrastructure.ViewModels;
using PS.Shell.ViewModels;
using PS.Shell.Views;
using PS.WPF.DataTemplate;

namespace PS.Shell
{
    public class MainModule : Autofac.Module
    {
        #region Override members

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);

            builder.RegisterType<NotificationView>();
            builder.RegisterType<NotificationViewModel>();
            builder.RegisterType<ConfirmationViewModel>();
        }

        #endregion

        #region Members

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.Associate<ShellViewModel>(
                       template: scope.Resolve<IDataTemplate<ShellView>>(),
                       style: XamlResources.ShellWindowStyle)
                   .Associate<NotificationViewModel>(
                       template: scope.Resolve<IDataTemplate<NotificationView>>(),
                       style: Infrastructure.XamlResources.NotificationStyle)
                   .Associate<ConfirmationViewModel>(
                       template: scope.Resolve<IDataTemplate<NotificationView>>(),
                       style: Infrastructure.XamlResources.ConfirmationStyle);
        }

        #endregion
    }
}