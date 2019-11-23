using System;
using System.Windows.Input;

namespace PS.Patterns.Command
{
    public class RelayCommand : RelayCommand<object>
    {
        #region Constructors

        public RelayCommand(Action executeAction, Func<bool> canExecutePredicate = null)
            :
            base(o => executeAction?.Invoke(), o => canExecutePredicate?.Invoke() ?? true)
        {
        }

        #endregion
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecutePredicate;
        private readonly Action<T> _executeAction;

        #region Constructors

        public RelayCommand(Action<T> executeAction = null, Func<T, bool> canExecutePredicate = null)
        {
            _executeAction = executeAction ?? (o => { });
            _canExecutePredicate = canExecutePredicate ?? (o => true);
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        #endregion

        #region Members

        public virtual bool CanExecute(T parameter)
        {
            return _canExecutePredicate(parameter);
        }

        public virtual void Execute(T parameter)
        {
            _executeAction?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}