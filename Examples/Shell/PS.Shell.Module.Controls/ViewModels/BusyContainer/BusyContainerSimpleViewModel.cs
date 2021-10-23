using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ControlsService;

namespace PS.Shell.Module.Controls.ViewModels.BusyContainer
{
    [DependencyRegisterAsSelf]
    public class BusyContainerSimpleViewModel : BaseNotifyPropertyChanged,
                                                IViewModel,
                                                IControlViewModel
    {
        private string _content;
        private bool _isBusy;

        #region Constructors

        public BusyContainerSimpleViewModel()
        {
            Title = "BusyContainer - simple";
            Group = "Controls";
        }

        #endregion

        #region Properties

        public string Content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetField(ref _isBusy, value); }
        }

        #endregion

        #region IControlViewModel Members

        public string Title { get; }
        public string Group { get; }

        #endregion
    }
}