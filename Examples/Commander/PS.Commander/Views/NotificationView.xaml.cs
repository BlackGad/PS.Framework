using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<NotificationViewModel>))]
    public partial class NotificationView : IView<NotificationViewModel>
    {
        #region Constructors

        public NotificationView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<NotificationViewModel> Members

        public NotificationViewModel ViewModel
        {
            get { return DataContext as NotificationViewModel; }
        }

        #endregion
    }
}