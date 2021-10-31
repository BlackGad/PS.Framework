using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ISourceFolder>))]
    public partial class SourceFolderView : IView<ISourceFolder>
    {
        #region Constructors

        public SourceFolderView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ISourceXaml> Members

        public ISourceFolder ViewModel
        {
            get { return DataContext as ISourceFolder; }
        }

        #endregion
    }
}