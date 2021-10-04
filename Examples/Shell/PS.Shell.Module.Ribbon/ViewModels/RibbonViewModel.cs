using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ControlsService;
using PS.WPF.Patterns.Command;

namespace PS.Shell.Module.Ribbon.ViewModels
{
    [DependencyRegisterAsSelf]
    public class RibbonViewModel : BaseNotifyPropertyChanged,
                                   IControlViewModel,
                                   IViewModel
    {
        #region Constructors

        public RibbonViewModel()
        {
            Title = "Ribbon";
            Group = "Controls";
            SomeCommand = new RelayUICommand();
        }

        #endregion
        public IUICommand SomeCommand { get; }
        #region IControlViewModel Members

        public string Group { get; }
        public string Title { get; }

        #endregion
    }
}