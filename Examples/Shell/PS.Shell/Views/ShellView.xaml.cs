using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.ViewModels;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ShellViewModel>))]
    public partial class ShellView : IView<ShellViewModel>
    {
        #region Constructors

        public ShellView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public ShellViewModel ViewModel
        {
            get { return DataContext as ShellViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}