using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    public partial class DesignView : IView<IExample>
    {
        #region Constructors

        public DesignView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<IExample> Members

        public IExample ViewModel
        {
            get { return DataContext as IExample; }
        }

        #endregion
    }
}