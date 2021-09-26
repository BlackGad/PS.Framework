using System;
using Autofac;
using PS.Commander.Models.ExplorerService;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ShellViewModel : BaseNotifyPropertyChanged,
                                  ITitleAware,
                                  IViewModel
    {
        #region Constructors

        public ShellViewModel(ILifetimeScope scope, ExplorerService explorerService)
        {
            if (scope == null) throw new ArgumentNullException(nameof(scope));
            if (explorerService == null) throw new ArgumentNullException(nameof(explorerService));

            Title = App.GetApplicationTitle();

            LeftArea = scope.Resolve<WorkingAreaViewModel>(TypedParameter.From(explorerService.GetExplorers(Area.Left)));
            RightArea = scope.Resolve<WorkingAreaViewModel>(TypedParameter.From(explorerService.GetExplorers(Area.Right)));
        }

        #endregion

        #region Properties

        public WorkingAreaViewModel LeftArea { get; }
        public WorkingAreaViewModel RightArea { get; }

        #endregion

        #region ITitleAware Members

        public string Title { get; }

        #endregion
    }
}