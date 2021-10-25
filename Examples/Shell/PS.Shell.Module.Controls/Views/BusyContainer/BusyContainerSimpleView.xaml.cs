using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels.BusyContainer;

namespace PS.Shell.Module.Controls.Views.BusyContainer
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<BusyContainerSimpleViewModel>))]
    public partial class BusyContainerSimpleView : IView<BusyContainerSimpleViewModel>
    {
        #region Constructors

        public BusyContainerSimpleView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<BusyContainerSimpleViewModel> Members

        public BusyContainerSimpleViewModel ViewModel
        {
            get { return DataContext as BusyContainerSimpleViewModel; }
        }

        #endregion
    }
}