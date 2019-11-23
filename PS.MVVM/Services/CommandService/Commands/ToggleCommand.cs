using System.Windows.Input;
using PS.Extensions;
using PS.Patterns.Aware;
using PS.Patterns.Command;

namespace PS.MVVM.Services.CommandService.Commands
{
    public class ToggleCommand : CommandServiceCommand,
                                 ICommandAware,
                                 IIsSelectedAware
    {
        private ICommand _command;
        private bool _isSelected;

        #region ICommandAware Members

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

        #endregion

        #region IIsSelectedAware Members

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected.AreEqual(value)) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}