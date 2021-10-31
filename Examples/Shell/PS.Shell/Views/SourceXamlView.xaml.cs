using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ISourceXaml>))]
    public partial class SourceXamlView : IView<ISourceXaml>
    {
        #region Constructors

        public SourceXamlView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ISourceXaml> Members

        public ISourceXaml ViewModel
        {
            get { return DataContext as ISourceXaml; }
        }

        #endregion
    }
}