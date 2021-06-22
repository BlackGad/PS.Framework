using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ConfirmationViewModel>))]
    public partial class ConfirmationView : IView<ConfirmationViewModel>
    {
        #region Constructors

        public ConfirmationView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ConfirmationViewModel> Members

        public ConfirmationViewModel ViewModel
        {
            get { return DataContext as ConfirmationViewModel; }
        }

        #endregion
    }
}