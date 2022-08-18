using PS.IoC.Attributes;
using PS.WPF.Commands;
using PS.WPF.Patterns.Command;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ConfirmationViewModel : NotificationViewModel
    {
        public ConfirmationViewModel()
        {
            CancelCommand = new CloseDialogCommand
            {
                Title = "Cancel",
                DialogResult = true,
                IsDefault = true
            };
        }

        public IUICommand CancelCommand { get; }
    }
}
