using System.Windows;
using System.Windows.Media;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;
using PS.WPF.Theming;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ButtonsViewModel>))]
    public partial class ButtonsView : IView<ButtonsViewModel>
    {
        public ButtonsView()
        {
            InitializeComponent();
        }

        public ButtonsViewModel ViewModel
        {
            get { return DataContext as ButtonsViewModel; }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Logger.Info("Button Button click event");
            Theme.Current.Colors.Accent = Colors.RoyalBlue;
        }

        private void SplitButton_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Logger.Info("Split Menu Button Button checked event");
        }

        private void SplitButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Logger.Info("Split Menu Button click event");
        }

        private void SplitButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Logger.Info("Split Menu Button Button unchecked event");
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Logger.Info("Toggle Button Button checked event");
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Logger.Info("Toggle Button Button unchecked event");
        }
    }
}
