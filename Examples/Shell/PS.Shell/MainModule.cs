using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Infrastructure;
using PS.Shell.Infrastructure.Models.ExamplesService;
using PS.Shell.Infrastructure.ViewModels;
using PS.Shell.ViewModels;
using PS.Shell.Views;
using PS.WPF.DataTemplate;
using PS.WPF.ValueConverters;

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
            service.AssociateTemplate<IExample>(scope.Resolve<IDataTemplate<DesignView>>())
                   .AssociateTemplate<ISourceFolder>(scope.Resolve<IDataTemplate<SourceFolderView>>())
                   .AssociateTemplate<ISourceXaml>(scope.Resolve<IDataTemplate<SourceXamlView>>())
                   .AssociateTemplate<ISourceCSharp>(scope.Resolve<IDataTemplate<SourceCSharpView>>());

            service.Associate<ShellViewModel>(
                       template: scope.Resolve<IDataTemplate<ShellView>>(),
                       style: XamlResources.ShellWindowStyle)
                   .Associate<NotificationViewModel>(
                       template: scope.Resolve<IDataTemplate<NotificationView>>(),
                       style: Infrastructure.XamlResources.NotificationStyle)
                   .Associate<ConfirmationViewModel>(
                       template: scope.Resolve<IDataTemplate<NotificationView>>(),
                       style: Infrastructure.XamlResources.ConfirmationStyle);

            var treeViewHierarchyBinding = TypedParameter.From<BindingBase>(new Binding
            {
                Path = new PropertyPath(nameof(ISource.Children)),
                Converter = CollectionConverters.Sort,
                ConverterParameter = new[]
                {
                    new SortDescription(nameof(ISource.Order), ListSortDirection.Ascending),
                    new SortDescription(nameof(ISource.Title), ListSortDirection.Ascending)
                }
            });

            service.Region(Regions.ShellTreeItem)
                   .AssociateTemplate<IExample>(scope.Resolve<IDataTemplate<TreeItemExampleView>>(treeViewHierarchyBinding))
                   .AssociateTemplate<ISourceFolder>(scope.Resolve<IDataTemplate<TreeItemSourceFolderView>>(treeViewHierarchyBinding))
                   .AssociateTemplate<ISourceXaml>(scope.Resolve<IDataTemplate<TreeItemSourceXamlView>>(treeViewHierarchyBinding))
                   .AssociateTemplate<ISourceCSharp>(scope.Resolve<IDataTemplate<TreeItemSourceCSharpView>>(treeViewHierarchyBinding));
        }

        #endregion
    }
}