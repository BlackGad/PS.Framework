using System.Windows.Input;
using PS.Extensions;
using PS.Patterns.Aware;
using PS.Patterns.Command;

namespace PS.MVVM.Services.CommandService.Commands
{
    public class ActionCommand : CommandServiceCommand,
                                 ICommandAware,
                                 ICommandParameterAware
    {
        private ICommand _command;
        private object _commandParameter;

        public ICommand Command
        {
            get { return _command ?? new RelayCommand(() => { }, () => false); }
            set
            {
                if (_command.AreEqual(value)) return;
                _command = value;
                OnPropertyChanged();
            }
        }

        public object CommandParameter
        {
            get { return _commandParameter; }
            set
            {
                if (_commandParameter.AreEqual(value)) return;
                _commandParameter = value;
                OnPropertyChanged();
            }
        }
    }
}
