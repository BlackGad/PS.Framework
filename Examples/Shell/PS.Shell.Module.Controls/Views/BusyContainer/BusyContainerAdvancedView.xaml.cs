using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels.BusyContainer;

namespace PS.Shell.Module.Controls.Views.BusyContainer
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<BusyContainerAdvancedViewModel>))]
    public partial class BusyContainerAdvancedView : IView<BusyContainerAdvancedViewModel>
    {
        public BusyContainerAdvancedView()
        {
            InitializeComponent();
        }

        public BusyContainerAdvancedViewModel ViewModel
        {
            get { return DataContext as BusyContainerAdvancedViewModel; }
        }
    }
}
