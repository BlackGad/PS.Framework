using System;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
using PS.Shell.Infrastructure.Models;
using PS.Shell.Infrastructure.Models.ControlsService;

namespace PS.Shell.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ShellViewModel : BaseNotifyPropertyChanged,
                                  ITitleAware,
                                  IViewModel
    {
        private object _content;

        #region Constructors

        public ShellViewModel(IControlsService controlsService)
        {
            ControlsService = controlsService ?? throw new ArgumentNullException(nameof(controlsService));
            Title = App.GetApplicationTitle();
        }

        #endregion

        #region Properties

        public object Content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }

        public IControlsService ControlsService { get; }

        #endregion

        #region ITitleAware Members

        public string Title { get; }

        #endregion
    }
}