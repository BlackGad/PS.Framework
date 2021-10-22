using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ControlsService;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class BusyContainerViewModel : BaseNotifyPropertyChanged,
                                          IViewModel,
                                          IControlViewModel
    {
        #region Constructors

        public BusyContainerViewModel()
        {
            Title = "BusyContainer";
            Group = "Controls";
        }

        #endregion

        #region IControlViewModel Members

        public string Title { get; }
        public string Group { get; }

        #endregion
    }
}