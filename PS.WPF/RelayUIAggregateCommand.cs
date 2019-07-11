using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.Navigation;
using PS.Extensions;

namespace PS.WPF
{
    public class RelayUIAggregateCommand : RelayUIAggregateCommand<object>
    {
        #region Constructors

        public RelayUIAggregateCommand(Action executeAction = null, Func<bool> canExecutePredicate = null)
            : base(o => executeAction?.Invoke(), o => canExecutePredicate?.Invoke() ?? true)
        {
        }

        #endregion
    }

    public class RelayUIAggregateCommand<T> : RelayUICommand<T>,
                                              IUICompositeCommand
    {
        private IList _commands;
        private DeepTracker _commandsTracker;

        #region Constructors

        public RelayUIAggregateCommand(Action<T> executeAction = null, Func<T, bool> canExecutePredicate = null)
            : base(executeAction, canExecutePredicate)
        {
            Children = new ObservableCollection<IUICommand>();
            _commandsTracker = DeepTracker.Setup(this, nameof(Children), Routes.Wildcard)
                                          .Subscribe<ObjectAttachmentEventArgs>(CommandAttachmentEvent)
                                          .Create()
                                          .Activate();
        }

        #endregion

        #region Override members

        public override bool CanExecute(T parameter)
        {
            var initialCanExecute = base.CanExecute(parameter);
            return Children.Enumerate<ICommand>()
                           .Aggregate(initialCanExecute, (agg, command) => agg && command.CanExecute(parameter));
        }

        public override void Execute(T parameter)
        {
            if (!CanExecute(parameter)) return;

            base.Execute(parameter);

            foreach (var command in Children.Enumerate<ICommand>())
            {
                command.Execute(parameter);
            }
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

        #region Event handlers

        private void CommandAttachmentEvent(object sender, ObjectAttachmentEventArgs e)
        {
            if (e.Object is ICommand command && !command.AreEqual(this))
            {
                if (e is ObjectAttachedEventArgs) command.CanExecuteChanged += CommandOnCanExecuteChanged;
                if (e is ObjectDetachedEventArgs) command.CanExecuteChanged -= CommandOnCanExecuteChanged;
            }
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs e)
        {
            RaiseCanExecuteChanged();
        }

        #endregion
    }
}