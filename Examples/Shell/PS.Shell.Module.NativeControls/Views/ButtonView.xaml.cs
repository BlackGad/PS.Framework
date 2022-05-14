using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.NativeControls.ViewModels;

namespace PS.Shell.Module.NativeControls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ButtonViewModel>))]
    public partial class ButtonView : IView<ButtonViewModel>
    {
        #region Constructors

        public ButtonView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ButtonViewModel> Members

        public ButtonViewModel ViewModel
        {
            get { return DataContext as ButtonViewModel; }
        }

        #endregion
    }
}