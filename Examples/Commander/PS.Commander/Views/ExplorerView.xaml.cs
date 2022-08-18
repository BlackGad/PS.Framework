using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ExplorerViewModel>))]
    public partial class ExplorerView : IView<ExplorerViewModel>
    {
        public ExplorerView()
        {
            InitializeComponent();
        }

        public ExplorerViewModel ViewModel
        {
            get { return DataContext as ExplorerViewModel; }
        }
    }
}
