using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.NativeControls.ViewModels;

namespace PS.Shell.Module.NativeControls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ButtonViewModel>))]
    public partial class ButtonView : IView<ButtonViewModel>
    {
        public ButtonView()
        {
            InitializeComponent();
        }

        public ButtonViewModel ViewModel
        {
            get { return DataContext as ButtonViewModel; }
        }
    }
}
