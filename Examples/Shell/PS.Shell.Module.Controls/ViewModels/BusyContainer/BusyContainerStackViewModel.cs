using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.WPF.Controls.BusyContainer;
using PS.WPF.Patterns.Command;

namespace PS.Shell.Module.Controls.ViewModels.BusyContainer
{
    [DependencyRegisterAsSelf]
    public class BusyContainerStackViewModel : BaseNotifyPropertyChanged,
                                               IViewModel
    {
        private IBusyState _firstState;
        private IBusyState _secondState;
        private IBusyState _thirdState;

        #region Constructors

        public BusyContainerStackViewModel(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Content = new StackBusyState();

            FirstStateCommands = new List<IUICommand>
            {
                new RelayUICommand(() => FirstState = Content.Push("First state title", "First state description"), () => FirstState == null)
                {
                    Title = "Initialize"
                },
                new RelayUICommand(() =>
                                   {
                                       FirstState.Dispose();
                                       FirstState = null;
                                   },
                                   () => FirstState != null)
                {
                    Title = "Reset"
                },
                new RelayUICommand(async () => await PayloadBackgroundOperation(FirstState),
                                   () => FirstState != null)
                {
                    Title = "Payload"
                }
            };

            SecondStateCommands = new List<IUICommand>
            {
                new RelayUICommand(() => SecondState = Content.Push("Second state title", "Second state description"), () => SecondState == null)
                {
                    Title = "Initialize"
                },
                new RelayUICommand(() =>
                                   {
                                       SecondState.Dispose();
                                       SecondState = null;
                                   },
                                   () => SecondState != null)
                {
                    Title = "Reset"
                }
            };

            ThirdStateCommands = new List<IUICommand>
            {
                new RelayUICommand(() => ThirdState = Content.Push("Third state title", "Third state description"), () => ThirdState == null)
                {
                    Title = "Initialize"
                },
                new RelayUICommand(() =>
                                   {
                                       ThirdState.Dispose();
                                       ThirdState = null;
                                   },
                                   () => ThirdState != null)
                {
                    Title = "Reset"
                }
            };
        }

        #endregion

        #region Properties

        public StackBusyState Content { get; }

        public IBusyState FirstState
        {
            get { return _firstState; }
            set
            {
                if (SetField(ref _firstState, value))
                {
                    foreach (var command in FirstStateCommands)
                    {
                        command.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public IList<IUICommand> FirstStateCommands { get; }
        public ILogger Logger { get; }

        public IBusyState SecondState
        {
            get { return _secondState; }
            set
            {
                if (SetField(ref _secondState, value))
                {
                    foreach (var command in SecondStateCommands)
                    {
                        command.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public IList<IUICommand> SecondStateCommands { get; }

        public IBusyState ThirdState
        {
            get { return _thirdState; }
            set
            {
                if (SetField(ref _thirdState, value))
                {
                    foreach (var command in ThirdStateCommands)
                    {
                        command.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public IList<IUICommand> ThirdStateCommands { get; }

        #endregion

        #region Members

        private async Task PayloadBackgroundOperation(IBusyState state)
        {
            state.Description = "Direct payload progress";
            await Task.Run(() =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(1000);
                    state.Description = $"Operation progress {i + 1} seconds passed";
                }
            });
            state.Description = "Direct payload progress ends";
        }

        #endregion
    }
}