
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    public partial class TreeItemSourceFolderView : IView<ISourceFolder>
    {
        #region Constructors

        public TreeItemSourceFolderView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public ISourceFolder ViewModel
        {
            get { return DataContext as ISourceFolder; }
        }

        #endregion
    }
}