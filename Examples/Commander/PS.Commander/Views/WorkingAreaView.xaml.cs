using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PS.Commander.Models.ExplorerService;
using PS.Commander.ViewModels;
using PS.Extensions;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<WorkingAreaViewModel>))]
    public partial class WorkingAreaView : IView<WorkingAreaViewModel>
    {
        private static ExplorerViewModel ExtractFilesViewModel(TabItem tabItem)
        {
            if (tabItem.DataContext is ExplorerViewModel filesViewModel)
            {
                return filesViewModel;
            }

            return null;
        }

        private readonly ExplorerService _explorerService;

        private Point? _dragStartPoint;

        public WorkingAreaView(ExplorerService explorerService)
        {
            _explorerService = explorerService ?? throw new ArgumentNullException(nameof(explorerService));
            InitializeComponent();
        }

        public WorkingAreaViewModel ViewModel
        {
            get { return DataContext as WorkingAreaViewModel; }
        }

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Source is TabItem tabItem)
            {
                var sourceExplorerViewModel = e.Data.GetData(typeof(ExplorerViewModel)) as ExplorerViewModel;
                var targetExplorerViewModel = ExtractFilesViewModel(tabItem);

                var relativePosition = (tabItem.ActualWidth - e.GetPosition(tabItem).X) / tabItem.ActualWidth;
                MoveExplorerViewModel(sourceExplorerViewModel, relativePosition < 0.5, targetExplorerViewModel);
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

        private void MoveExplorerViewModel(ExplorerViewModel source, bool beforeTarget, ExplorerViewModel target)
        {
            if (source.AreEqual(target)) return;

            //Remove source item first
            _explorerService.ExplorerViewModels.Remove(source);

            //Determine target element index in flat collection
            var targetFlatIndex = _explorerService.ExplorerViewModels.IndexOf(target);

            //Offset index if we need place source item after target item
            if (!beforeTarget)
            {
                targetFlatIndex++;
            }

            //Synchronize items area
            source.Container = target.Container;

            //Insert source item before or after target
            _explorerService.ExplorerViewModels.SafeInsert(targetFlatIndex, source);
        }
    }
}
