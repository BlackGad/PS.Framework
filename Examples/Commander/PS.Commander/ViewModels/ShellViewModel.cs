using System;
using Autofac;
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

        public ShellViewModel(ILifetimeScope scope)
        {
            if (scope == null) throw new ArgumentNullException(nameof(scope));

            Title = App.GetApplicationTitle();

            Left = scope.Resolve<WorkingAreaViewModel>(TypedParameter.From(nameof(Left)));
            Right = scope.Resolve<WorkingAreaViewModel>(TypedParameter.From(nameof(Right)));
        }

        #endregion

        #region Properties

        public WorkingAreaViewModel Left { get; }
        public WorkingAreaViewModel Right { get; }

        #endregion

        #region ITitleAware Members

        public string Title { get; }

        #endregion
    }
}