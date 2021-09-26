using System;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.MVVM.Services;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class WorkingAreaViewModel : BaseNotifyPropertyChanged,
                                        IViewModel
    {
        #region Constructors

        public WorkingAreaViewModel(IObservableModelCollection explorerViews)
        {
            ExplorerViews = explorerViews ?? throw new ArgumentNullException(nameof(explorerViews));
        }

        #endregion

        #region Properties

        public ExplorerViewModel SelectedExplorerView
        {
            get { return _selectedExplorerView; }
            set { SetField(ref _selectedExplorerView, value); }
        }

        private ExplorerViewModel _selectedExplorerView;
        public IObservableModelCollection ExplorerViews { get; }

        #endregion
    }
}