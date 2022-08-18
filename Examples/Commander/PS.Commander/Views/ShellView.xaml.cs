using System.Windows;
using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ShellViewModel>))]
    public partial class ShellView : IView<ShellViewModel>
    {
        public ShellView()
        {
            InitializeComponent();
        }

        public ShellViewModel ViewModel
        {
            get { return DataContext as ShellViewModel; }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotificationException("Hi!", "With title");
        }
    }
}
