using System.Collections.ObjectModel;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ControlsService;
using PS.WPF.Patterns.Command;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ButtonsViewModel : BaseNotifyPropertyChanged,
                                    IViewModel,
                                    IControlViewModel
    {
        #region Constructors

        public ButtonsViewModel()
        {
            Title = "Buttons";
            Group = "Controls";
            Log = new ObservableCollection<string>();

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

        #endregion

        #region Properties

        public IUICommand ButtonAction { get; }
        public ObservableCollection<string> Log { get; }
        public IUICommand SplitButtonAction { get; }
        public ObservableCollection<IUICommand> SplitMenuButtonCommands { get; }
        public IUICommand ToggleButtonAction { get; }

        #endregion

        #region IControlViewModel Members

        public string Title { get; }
        public string Group { get; }

        #endregion

        #region Members

        private IUICommand CreateUICommand(string name, bool canExecute = true)
        {
            return new RelayUICommand(() => Log.Add($"Command '{name}' execute triggered"), () => canExecute)
            {
                Title = name
            };
        }

        private IUICommand CreateUICommand<T>(string name, bool canExecute = true)
        {
            return new RelayUICommand<T>(p => Log.Add($"Command '{name}' execute triggered with {p} param"), p => canExecute)
            {
                Title = name
            };
        }

        #endregion
    }
}