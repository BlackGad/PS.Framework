using System;
using NLog;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.Controls.ViewModels.BusyContainer
{
    [DependencyRegisterAsSelf]
    public class BusyContainerSimpleViewModel : BaseNotifyPropertyChanged,
                                                IViewModel
    {
        private string _content;
        private bool _isBusy;

        #region Constructors

        public BusyContainerSimpleViewModel(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public ILogger Logger { get; }

        #endregion
    }
}