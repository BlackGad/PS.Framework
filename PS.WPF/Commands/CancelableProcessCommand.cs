using System;
using System.Threading;
using System.Threading.Tasks;
using PS.WPF.Patterns.Command;

namespace PS.WPF.Commands
{
    public class CancelableProcessCommand : CancelableProcessCommand<object>
    {
        public CancelableProcessCommand(Func<CancellationToken, Task> executeAction = null,
                                        Func<bool> canExecutePredicate = null,
                                        Action<Exception> errorAction = null)
            : base((parameter, token) => executeAction?.Invoke(token),
                   parameter => canExecutePredicate?.Invoke() ?? true,
                   (parameter, e) => errorAction?.Invoke(e))
        {
        }
    }

    public class CancelableProcessCommand<T> : BaseNotifyPropertyChanged,
                                               IUICommand
    {
        private readonly Action<T, Exception> _errorAction;
        private readonly Func<T, bool> _userCanExecutePredicate;
        private readonly Func<T, CancellationToken, Task> _userExecuteAction;
        private Func<T, bool> _activeCanExecutePredicate;
        private Func<T, Task> _activeExecuteAction;

        private object _cancelColor;
        private string _cancelDescription;
        private object _cancelIcon;
        private string _cancelTitle;

        private object _color;
        private string _description;
        private string _group;
        private object _icon;

        private bool _isInProgress;
        private bool _isVisible;
        private int _order;
        private string _title;

        public CancelableProcessCommand(Func<T, CancellationToken, Task> executeAction = null,
                                        Func<T, bool> canExecutePredicate = null,
                                        Action<T, Exception> errorAction = null)
        {
            _errorAction = errorAction ?? ((parameter, e) => throw e);
            _userExecuteAction = executeAction ?? ((parameter, token) => Task.CompletedTask);
            _userCanExecutePredicate = canExecutePredicate ?? (parameter => true);

            _activeCanExecutePredicate = _userCanExecutePredicate;
            _activeExecuteAction = Process;

            IsVisible = true;
        }

        public object CancelColor
        {
            get { return _cancelColor; }
            set { SetField(ref _cancelColor, value); }
        }

        public string CancelDescription
        {
            get { return _cancelDescription; }
            set { SetField(ref _cancelDescription, value); }
        }

        public object CancelIcon
        {
            get { return _cancelIcon; }
            set { SetField(ref _cancelIcon, value); }
        }

        public string CancelTitle
        {
            get { return _cancelTitle; }
            set { SetField(ref _cancelTitle, value); }
        }

        public bool IsInProgress
        {
            get { return _isInProgress; }
            set { SetField(ref _isInProgress, value); }
        }

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

        private async Task Process(T parameter)
        {
            if (!_userCanExecutePredicate(parameter))
            {
                return;
            }

            var initialTitle = Title;
            var initialDescription = Description;
            var initialIcon = Icon;
            var initialColor = Color;
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

                Title = CancelTitle ?? Title;
                Description = CancelDescription ?? Description;
                Icon = CancelIcon ?? Icon;
                Color = CancelColor ?? Color;
                RaiseCanExecuteChanged();

                token.Register(() =>
                {
                    Title = "Cancelling...";
                    RaiseCanExecuteChanged();
                });
                IsInProgress = true;
                Exception capturedException = null;
                await Task.Run(async () =>
                               {
                                   try
                                   {
                                       await _userExecuteAction(parameter, token);
                                   }
                                   catch (Exception e)
                                   {
                                       capturedException = e;
                                   }
                               },
                               token);

                if (capturedException != null)
                {
                    throw capturedException;
                }
            }
            catch (Exception e)
            {
                _errorAction(parameter, e);
            }
            finally
            {
                Title = initialTitle;
                Description = initialDescription;
                Icon = initialIcon;
                Color = initialColor;

                IsInProgress = false;

                _activeExecuteAction = Process;
                _activeCanExecutePredicate = _userCanExecutePredicate;

                RaiseCanExecuteChanged();
            }
        }
    }
}
