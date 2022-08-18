using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.ViewModels;

namespace PS.Shell.Views;

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
