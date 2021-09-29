using PS.IoC.Attributes;
using PS.WPF;
using PS.WPF.Commands;
using PS.WPF.Patterns.Command;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ConfirmationViewModel : NotificationViewModel
    {
        #region Constructors

        public ConfirmationViewModel()
        {
            CancelCommand = new CloseDialogCommand
            {
                Title = "Cancel",
                DialogResult = true,
                IsDefault = true
            };
        }

        #endregion

        #region Properties

        public IUICommand CancelCommand { get; }

        #endregion
    }
}