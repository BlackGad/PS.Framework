using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PS.Commander.Models.ExplorerService;
using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<WorkingAreaViewModel>))]
    public partial class WorkingAreaView : IView<WorkingAreaViewModel>
    {
        #region Static members

        private static ExplorerViewModel ExtractFilesViewModel(TabItem tabItem)
        {
            if (tabItem.DataContext is ExplorerViewModel filesViewModel)
            {
                return filesViewModel;
            }

            return null;
        }

        #endregion

        private readonly ExplorerService _explorerService;

        private Point? _dragStartPoint;

        #region Constructors

        public WorkingAreaView(ExplorerService explorerService)
        {
            _explorerService = explorerService ?? throw new ArgumentNullException(nameof(explorerService));
            InitializeComponent();
        }

        #endregion

        #region IView<WorkingAreaViewModel> Members

        public WorkingAreaViewModel ViewModel
        {
            get { return DataContext as WorkingAreaViewModel; }
        }

        #endregion

        #region Event handlers

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Source is TabItem tabItem)
            {
                var sourceFilesViewModel = e.Data.GetData(typeof(ExplorerViewModel)) as ExplorerViewModel;
                var targetFilesViewModel = ExtractFilesViewModel(tabItem);

                var relativePosition = (tabItem.ActualWidth - e.GetPosition(tabItem).X) / tabItem.ActualWidth;
                var placeType = relativePosition < 0.5
                    ? Place.Before
                    : Place.After;

                _explorerService.PlaceExplorer(sourceFilesViewModel, placeType, targetFilesViewModel);
            }
        }

        private void TabItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is TabItem)
            {
                _dragStartPoint = e.GetPosition(this);
            }
        }

        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is TabItem tabItem && ExtractFilesViewModel(tabItem) is ExplorerViewModel filesViewModel)
            {
                if (_dragStartPoint == null) return;

                var vector = e.GetPosition(this) - _dragStartPoint.Value;

                if (vector.Length > 3)
                {
                    DragDrop.DoDragDrop(this, filesViewModel, DragDropEffects.Move);
                    _dragStartPoint = null;
                }
            }
        }

        #endregion
    }
}