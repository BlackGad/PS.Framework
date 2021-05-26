using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.ViewModels;

namespace PS.Shell.Infrastructure.Views
{
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
            set { DataContext = value; }
        }

        #endregion
    }
}