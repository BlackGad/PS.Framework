using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    public partial class TreeItemSourceXamlView : IView<ISourceXaml>
    {
        public TreeItemSourceXamlView()
        {
            InitializeComponent();
        }

        public ISourceXaml ViewModel
        {
            get { return DataContext as ISourceXaml; }
        }
    }
}
