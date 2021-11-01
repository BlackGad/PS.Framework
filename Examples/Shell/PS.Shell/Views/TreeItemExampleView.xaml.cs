
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    public partial class TreeItemExampleView : IView<IExample>
    {
        #region Constructors

        public TreeItemExampleView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public IExample ViewModel
        {
            get { return DataContext as IExample; }
        }

        #endregion
    }
}