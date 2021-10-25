using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels.BusyContainer;

namespace PS.Shell.Module.Controls.Views.BusyContainer
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<BusyContainerAdvancedViewModel>))]
    public partial class BusyContainerAdvancedView : IView<BusyContainerAdvancedViewModel>
    {
        #region Constructors

        public BusyContainerAdvancedView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<BusyContainerAdvancedViewModel> Members

        public BusyContainerAdvancedViewModel ViewModel
        {
            get { return DataContext as BusyContainerAdvancedViewModel; }
        }

        #endregion
    }
}