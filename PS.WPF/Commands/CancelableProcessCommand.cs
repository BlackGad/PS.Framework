using System;
using System.Threading;
using System.Threading.Tasks;
using PS.WPF.Patterns.Command;

namespace PS.WPF.Commands
{
    public class CancelableProcessCommand : CancelableProcessCommand<object>
    {
        #region Constructors

        public CancelableProcessCommand(Action<CancellationToken> executeAction = null,
                                        Func<bool> canExecutePredicate = null,
                                        Action<Exception> errorAction = null)
            : base((parameter, token) => executeAction?.Invoke(token),
                   parameter => canExecutePredicate?.Invoke() ?? true,
                   (parameter, e) => errorAction?.Invoke(e))
        {
        }

        #endregion
    }

    public class CancelableProcessCommand<T> : BaseNotifyPropertyChanged,
                                               IUICommand
    {
        private readonly Action<T, Exception> _errorAction;
        private readonly Func<T, bool> _userCanExecutePredicate;
        private readonly Action<T, CancellationToken> _userExecuteAction;
        private Func<T, bool> _activeCanExecutePredicate;
        private Func<T, Task> _activeExecuteAction;

        private object _color;
        private string _description;
        private string _group;
        private object _icon;
        private bool _isVisible;
        private int _order;
        private string _title;

        #region Constructors

        public CancelableProcessCommand(Action<T, CancellationToken> executeAction = null,
                                        Func<T, bool> canExecutePredicate = null,
                                        Action<T, Exception> errorAction = null)
        {
            _errorAction = errorAction ?? ((parameter, e) => throw e);
            _userExecuteAction = executeAction ?? ((parameter, token) => { });
            _userCanExecutePredicate = canExecutePredicate ?? (parameter => true);

            _activeCanExecutePredicate = _userCanExecutePredicate;
            _activeExecuteAction = Process;

            IsVisible = true;
        }

        #endregion

        #region IUICommand Members

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetField(ref _isVisible, value); }
        }

        public bool CanExecute(object parameter)
        {
            return _activeCanExecutePredicate.Invoke((T)parameter);
        }

        public async void Execute(object parameter)
        {
            await _activeExecuteAction((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        public string Group
        {
            get { return _group; }
            set { SetField(ref _group, value); }
        }

        public int Order
        {
            get { return _order; }
            set { SetField(ref _order, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        public object Icon
        {
            get { return _icon; }
            set { SetField(ref _icon, value); }
        }

        public object Color
        {
            get { return _color; }
            set { SetField(ref _color, value); }
        }

        #endregion

        #region Members

        private async Task Process(T parameter)
        {
            if (!_userCanExecutePredicate(parameter))
            {
                return;
            }

            var initialTitle = Title;
            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;

                _activeExecuteAction = _ =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        cancellationTokenSource.Cancel();
                    }

                    return Task.CompletedTask;
                };
                _activeCanExecutePredicate = _ => !token.IsCancellationRequested;

                Title = "Cancel";

                RaiseCanExecuteChanged();

                token.Register(() =>
                {
                    Title = "Cancelling...";
                    RaiseCanExecuteChanged();
                });

                await Task.Run(() => _userExecuteAction(parameter, token), token);
            }
            catch (Exception e)
            {
                _errorAction(parameter, e);
            }
            finally
            {
                Title = initialTitle;
                _activeExecuteAction = Process;
                _activeCanExecutePredicate = _userCanExecutePredicate;

                RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}