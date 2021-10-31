using System.Collections.Generic;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
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

        #region Constructors

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

        #endregion

        #region Properties

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

        #endregion

        #region Nested type: CustomState

        public class CustomState : BaseNotifyPropertyChanged,
                                   IMutableTitleAware
        {
            private string _title;

            #region Constructors

            public CustomState()
            {
                Title = "Mutable title";
            }

            #endregion

            #region IMutableTitleAware Members

            public string Title
            {
                get { return _title; }
                set { SetField(ref _title, value); }
            }

            #endregion
        }

        #endregion

        #region Nested type: StateWithMutableDescription

        public class StateWithMutableDescription : BaseNotifyPropertyChanged,
                                                   IMutableDescriptionAware
        {
            private string _description;

            #region Constructors

            public StateWithMutableDescription()
            {
                Description = "Mutable description";
            }

            #endregion

            #region IMutableDescriptionAware Members

            public string Description
            {
                get { return _description; }
                set { SetField(ref _description, value); }
            }

            #endregion
        }

        #endregion

        #region Nested type: StateWithToStringOverride

        public class StateWithToStringOverride
        {
            #region Override members

            public override string ToString()
            {
                return "Custom state with immutable description";
            }

            #endregion
        }

        #endregion
    }
}