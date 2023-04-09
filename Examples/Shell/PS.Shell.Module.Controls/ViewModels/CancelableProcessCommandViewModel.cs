using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure;
using PS.WPF.Commands;
using PS.WPF.Extensions;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class CancelableProcessCommandViewModel : BaseNotifyPropertyChanged,
                                                     IViewModel
    {
        private readonly Dispatcher _dispatcher;
        private bool _canExecute;

        private string _progress;

        private Visibility _progressVisibility;

        public CancelableProcessCommandViewModel()
        {
            _dispatcher = Application.Current.Dispatcher;

            ProgressVisibility = Visibility.Hidden;
            Command = new CancelableProcessCommand(ExecuteAction, CanExecutePredicate, ErrorAction)
            {
                Title = "Title",
                Description = "Description"
            };
        }

        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (SetField(ref _canExecute, value))
                {
                    Command.RaiseCanExecuteChanged();
                }
            }
        }

        public CancelableProcessCommand Command { get; }

        public string Progress
        {
            get { return _progress; }
            set { _dispatcher.SafeCall(() => SetField(ref _progress, value)); }
        }

        public Visibility ProgressVisibility
        {
            get { return _progressVisibility; }
            set { _dispatcher.SafeCall(() => SetField(ref _progressVisibility, value)); }
        }

        private bool CanExecutePredicate()
        {
            return CanExecute;
        }

        private void ErrorAction(Exception e)
        {
            throw new NotificationException("Process error", e);
        }

        private async Task ExecuteAction(CancellationToken token)
        {
            try
            {
                ProgressVisibility = Visibility.Visible;
                var max = 5;
                for (var i = 0; i < 5; i++)
                {
                    Progress = $"{i + 1}/{max}";
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1000, token);
                }
            }
            finally
            {
                ProgressVisibility = Visibility.Hidden;
            }
        }
    }
}
