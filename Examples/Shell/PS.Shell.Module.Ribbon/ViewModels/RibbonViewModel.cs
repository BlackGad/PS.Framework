using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.WPF.Patterns.Command;

namespace PS.Shell.Module.Ribbon.ViewModels
{
    [DependencyRegisterAsSelf]
    public class RibbonViewModel : BaseNotifyPropertyChanged,
                                   IViewModel
    {
        #region Constructors

        public RibbonViewModel()
        {
            SomeCommand = new RelayUICommand();
        }

        #endregion

        #region Properties

        public IUICommand SomeCommand { get; }

        #endregion
    }
}