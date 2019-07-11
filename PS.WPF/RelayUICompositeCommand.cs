using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace PS.WPF
{
    public class RelayUICompositeCommand : RelayUICompositeCommand<object>
    {
        #region Constructors

        public RelayUICompositeCommand(Action executeAction = null, Func<bool> canExecutePredicate = null)
            : base(o => executeAction?.Invoke(), o => canExecutePredicate?.Invoke() ?? true)
        {
        }

        #endregion
    }

    public class RelayUICompositeCommand<T> : RelayUICommand<T>,
                                              IUICompositeCommand
    {
        private IList _commands;

        #region Constructors

        public RelayUICompositeCommand(Action<T> executeAction = null, Func<T, bool> canExecutePredicate = null)
            : base(executeAction, canExecutePredicate)
        {
            Children = new ObservableCollection<IUICommand>();
        }

        #endregion

        #region IUICompositeCommand Members

        public IList Children
        {
            get { return _commands; }
            set
            {
                _commands = value;
                OnPropertyChangedAuto();
            }
        }

        #endregion
    }
}