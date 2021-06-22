using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ExplorerViewModel>))]
    public partial class ExplorerView : IView<ExplorerViewModel>
    {
        #region Constructors

        public ExplorerView()
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