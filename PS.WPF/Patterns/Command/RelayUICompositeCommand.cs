using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace PS.WPF.Patterns.Command
{
    public class RelayUICompositeCommand : RelayUICompositeCommand<object>
    {
        public RelayUICompositeCommand(Action executeAction = null, Func<bool> canExecutePredicate = null)
            : base(o => executeAction?.Invoke(), o => canExecutePredicate?.Invoke() ?? true)
        {
        }
    }

    public class RelayUICompositeCommand<T> : RelayUICommand<T>,
                                              IUICompositeCommand
    {
        private IList _commands;

        public RelayUICompositeCommand(Action<T> executeAction = null, Func<T, bool> canExecutePredicate = null)
            : base(executeAction, canExecutePredicate)
        {
            Children = new ObservableCollection<IUICommand>();
        }

        public IList Children
        {
            get { return _commands; }
            set
            {
                _commands = value;
                OnPropertyChangedAuto();
            }
        }
    }
}
