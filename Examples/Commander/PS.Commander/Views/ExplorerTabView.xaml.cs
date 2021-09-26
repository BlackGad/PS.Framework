using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ExplorerViewModel>))]
    public partial class ExplorerTabView : IView<ExplorerViewModel>
    {
        #region Constructors

        public ExplorerTabView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ExplorerViewModel> Members

        public ExplorerViewModel ViewModel
        {
            get { return DataContext as ExplorerViewModel; }
        }

        #endregion
    }
}