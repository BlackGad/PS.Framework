using System;
using NLog;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class TextBoxViewModel : BaseNotifyPropertyChanged,
                                    IViewModel
    {
        #region Constructors

        public TextBoxViewModel(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Properties

        public ILogger Logger { get; }

        #endregion
    }
}