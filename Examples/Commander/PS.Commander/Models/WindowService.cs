using System;
using System.Windows;
using Autofac;
using PS.IoC.Attributes;
using PS.MVVM.Services;
using PS.MVVM.Services.WindowService;

namespace PS.Commander.Models
{
    [DependencyRegisterAsInterface(typeof(IWindowService))]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    internal class WindowService : MVVM.Services.WindowService.WindowService
    {
        private readonly ILifetimeScope _scope;
        private readonly IViewResolverService _viewResolverService;

        #region Constructors

        public WindowService(IViewResolverService viewResolverService,
                             ILifetimeScope scope)
        {
            _viewResolverService = viewResolverService;
            _scope = scope;
        }

        #endregion

        #region Override members

        protected override Window CreateWindow()
        {
            return new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
        }

        protected override IViewAssociation GetAssociation(Type consumerServiceType, Type viewModelType, string key)
        {
            return _viewResolverService.Find(consumerServiceType, viewModelType, key);
        }

        protected override object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }

        #endregion
    }
}