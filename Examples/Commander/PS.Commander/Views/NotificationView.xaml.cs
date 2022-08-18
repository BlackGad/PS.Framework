using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<NotificationViewModel>))]
    public partial class NotificationView : IView<NotificationViewModel>
    {
        public NotificationView()
        {
            InitializeComponent();
        }

        public NotificationViewModel ViewModel
        {
            get { return DataContext as NotificationViewModel; }
        }
    }
}
