using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;

namespace PS.Shell.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ShellViewModel : BaseNotifyPropertyChanged,
                                  ITitleAware,
                                  IViewModel
    {
        #region Constructors

        public ShellViewModel()
        {
            Title = App.GetApplicationTitle();
        }

        #endregion

        #region ITitleAware Members

        public string Title { get; }

        #endregion
    }
}