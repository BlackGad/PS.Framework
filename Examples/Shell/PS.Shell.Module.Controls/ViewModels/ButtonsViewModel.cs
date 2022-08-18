using System;
using System.Collections.ObjectModel;
using NLog;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.WPF.Patterns.Command;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ButtonsViewModel : BaseNotifyPropertyChanged,
                                    IViewModel
    {
        public ButtonsViewModel(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            SplitMenuButtonCommands = new ObservableCollection<IUICommand>
            {
                CreateUICommand("Item 1"),
                CreateUICommand("Item 2", false),
                CreateUICommand("Item 3")
            };

            SplitButtonAction = CreateUICommand("Split Menu Button action");
            ButtonAction = CreateUICommand("Button action");
            ToggleButtonAction = CreateUICommand<bool>("Toggle Button action");
        }

        public IUICommand ButtonAction { get; }

        public ILogger Logger { get; }

        public IUICommand SplitButtonAction { get; }

        public ObservableCollection<IUICommand> SplitMenuButtonCommands { get; }

        public IUICommand ToggleButtonAction { get; }

        private IUICommand CreateUICommand(string name, bool canExecute = true)
        {
            return new RelayUICommand(() => Logger.Info($"Command '{name}' execute triggered"), () => canExecute)
            {
                Title = name
            };
        }

        private IUICommand CreateUICommand<T>(string name, bool canExecute = true)
        {
            return new RelayUICommand<T>(p => Logger.Info($"Command '{name}' execute triggered with {p} param"), p => canExecute)
            {
                Title = name
            };
        }
    }
}
