using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ControlsService;
using PS.WPF.Controls.BusyContainer;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class BusyContainerViewModel : BaseNotifyPropertyChanged,
                                          IViewModel,
                                          IControlViewModel
    {
        #region Constants

     

        #endregion

      
        #region Constructors

        public BusyContainerViewModel()
        {
            Title = "BusyContainer";
            Group = "Controls";

        }

        #endregion

        #region Properties

      

        #endregion

        #region IControlViewModel Members

        public string Title { get; }
        public string Group { get; }

        #endregion
    }
}