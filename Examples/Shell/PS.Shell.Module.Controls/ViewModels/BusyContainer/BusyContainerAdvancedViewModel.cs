using System.Collections.Generic;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.WPF.Controls.BusyContainer;
using PS.WPF.Patterns.Command;

namespace PS.Shell.Module.Controls.ViewModels.BusyContainer
{
    [DependencyRegisterAsSelf]
    public class BusyContainerAdvancedViewModel : BaseNotifyPropertyChanged,
                                                  IViewModel
    {
        private object _content;
        private bool _isBusy;

        public BusyContainerAdvancedViewModel()
        {
            ResetContentCommand = new RelayUICommand(() => Content = null, () => Content != null)
            {
                Title = "Reset"
            };

            ContentControlCommands = new[]
            {
                ResetContentCommand,
                new RelayUICommand(() => Content = new CustomState())
                {
                    Title = "Mutable title"
                },
                new RelayUICommand(() => Content = new StateWithMutableDescription())
                {
                    Title = "Mutable description"
                },
                new RelayUICommand(() => Content = new StateWithToStringOverride())
                {
                    Title = "ToString override"
                },
                new RelayUICommand(() => Content = new BusyState
                {
                    Title = "Built in type",
                    Description = "Some description"
                })
                {
                    Title = "Built in"
                }
            };

            SetContentStateWithMutableTitleCommand = new RelayUICommand(() => Content = new CustomState())
            {
                Title = "Mutable title"
            };
            SetContentStateWithMutableDescriptionCommand = new RelayUICommand(() => Content = new StateWithMutableDescription())
            {
                Title = "Mutable description"
            };
            SetContentStateWithToStringOverrideCommand = new RelayUICommand(() => Content = new StateWithToStringOverride())
            {
                Title = "ToString override"
            };
            SetContentBuiltInBusyStateCommand = new RelayUICommand(() => Content = new BusyState
            {
                Title = "Built in type",
                Description = "Some description"
            })
            {
                Title = "Built in"
            };
        }

        public object Content
        {
            get { return _content; }
            set
            {
                if (SetField(ref _content, value))
                {
                    ResetContentCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public IList<IUICommand> ContentControlCommands { get; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetField(ref _isBusy, value); }
        }

        public IUICommand ResetContentCommand { get; }

        public IUICommand SetContentBuiltInBusyStateCommand { get; }

        public IUICommand SetContentStateWithMutableDescriptionCommand { get; }

        public IUICommand SetContentStateWithMutableTitleCommand { get; }

        public IUICommand SetContentStateWithToStringOverrideCommand { get; }
    }
}
