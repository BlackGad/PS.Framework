using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ButtonsViewModel>))]
    public partial class ButtonsViewView : IView<ButtonsViewModel>
    {
        #region Constructors

        public ButtonsViewView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<SplitButtonViewModel> Members

        public ButtonsViewModel ViewModel
        {
            get { return DataContext as ButtonsViewModel; }
        }

        #endregion
    }
}