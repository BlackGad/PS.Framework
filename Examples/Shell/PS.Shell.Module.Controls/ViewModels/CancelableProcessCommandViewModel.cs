using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure;
using PS.Shell.Infrastructure.Models.ControlsService;
using PS.WPF.Commands;
using PS.WPF.Extensions;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class CancelableProcessCommandViewModel : BaseNotifyPropertyChanged,
                                                     IControlViewModel,
                                                     IViewModel
    {
        private readonly Dispatcher _dispatcher;
        private bool _canExecute;

        private string _progress;

        private Visibility _progressVisibility;

        #region Constructors

        public CancelableProcessCommandViewModel()
        {
            Title = "CancelableProcessCommand";
            Group = "Commands";

            _dispatcher = Application.Current.Dispatcher;

            ProgressVisibility = Visibility.Hidden;
            Command = new CancelableProcessCommand(ExecuteAction, CanExecutePredicate, ErrorAction)
            {
                Title = "Title",
                Description = "Description"
            };
        }

        #endregion

        #region Properties

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

        #endregion

        #region IControlViewModel Members

        public string Group { get; }
        public string Title { get; }

        #endregion

        #region Members

        private bool CanExecutePredicate()
        {
            return CanExecute;
        }

        private void ErrorAction(Exception e)
        {
            throw new NotificationException("Process error", e);
        }

        private void ExecuteAction(CancellationToken token)
        {
            try
            {
                ProgressVisibility = Visibility.Visible;
                var max = 5;
                for (var i = 0; i < 5; i++)
                {
                    Progress = $"{i + 1}/{max}";
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                ProgressVisibility = Visibility.Hidden;
            }
        }

        #endregion
    }
}