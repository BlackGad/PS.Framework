using System;
using System.ComponentModel;
using System.Windows.Data;
using PS.Commander.Models.ExplorerService;
using PS.Extensions;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class WorkingAreaViewModel : BaseNotifyPropertyChanged,
                                        IViewModel
    {
        private ExplorerViewModel _selectedExplorerView;
        private readonly CollectionViewSource _viewSource;

        #region Constructors

        public WorkingAreaViewModel(string containerName,
                                    ExplorerService explorerService)
        {
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            _viewSource = new CollectionViewSource
            {
                Source = explorerService.ExplorerViewModels,
                IsLiveFilteringRequested = true,
                LiveFilteringProperties = { nameof(ExplorerViewModel.Container) }
            };

            _viewSource.Filter += (sender, args) =>
            {
                args.Accepted = args.Item is ExplorerViewModel explorerViewModel && 
                                explorerViewModel.Container.AreEqual(containerName);
            };

            ExplorerViewModels = _viewSource.View;
        }

        #endregion

        #region Properties

        public ICollectionView ExplorerViewModels { get; }

        public ExplorerViewModel SelectedExplorerView
        {
            get { return _selectedExplorerView; }
            set { SetField(ref _selectedExplorerView, value); }
        }

        #endregion
    }
}