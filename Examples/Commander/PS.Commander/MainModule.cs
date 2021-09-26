using System.Windows;
using System.Windows.Threading;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.Commander.Models.ExplorerService;
using PS.Commander.ViewModels;
using PS.Commander.Views;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.WPF.DataTemplate;
using PS.WPF.Extensions;

namespace PS.Commander
{
    public class MainModule : Module
    {
        #region Override members

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
            registration.HandleActivation<ExplorerService>(FilesServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
        }

        #endregion

        #region Members

        private void FilesServiceActivation(ILifetimeScope scope, ExplorerService service)
        {
            Application.Current.Dispatcher.Postpone(DispatcherPriority.Background, service.Load);
        }

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            //Windows
            service.Associate<ShellViewModel>(
                       template: scope.Resolve<IDataTemplate<ShellView>>(),
                       style: XamlResources.ShellWindowStyle)
                   .Associate<NotificationViewModel>(
                       template: scope.Resolve<IDataTemplate<NotificationView>>(),
                       style: XamlResources.NotificationStyle)
                   .Associate<ConfirmationViewModel>(
                       template: scope.Resolve<IDataTemplate<ConfirmationView>>(),
                       style: XamlResources.NotificationStyle);

            //Generic views with default region
            service.AssociateTemplate<WorkingAreaViewModel>(scope.Resolve<IDataTemplate<WorkingAreaView>>())
                   .AssociateTemplate<ExplorerViewModel>(scope.Resolve<IDataTemplate<ExplorerView>>());

            //Region specific views
            service.Region(ViewRegions.WorkingAreaTab)
                   .AssociateTemplate<ExplorerViewModel>(scope.Resolve<IDataTemplate<ExplorerTabView>>());
        }

        #endregion
    }
}