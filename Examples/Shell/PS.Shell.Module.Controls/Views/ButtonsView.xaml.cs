using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ButtonsViewModel>))]
    public partial class ButtonsViewView : IView<ButtonsViewModel>
    {
        #region Constructors

        public ButtonsViewView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ButtonsViewModel> Members

        public ButtonsViewModel ViewModel
        {
            get { return DataContext as ButtonsViewModel; }
        }

        #endregion

        #region Event handlers

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Log.Add("Button Button click event");
        }

        private void SplitButton_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Log.Add("Split Menu Button Button checked event");
        }

        private void SplitButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Log.Add("Split Menu Button click event");
        }

        private void SplitButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Log.Add("Split Menu Button Button unchecked event");
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Log.Add("Toggle Button Button checked event");
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Log.Add("Toggle Button Button unchecked event");
        }

        #endregion
    }
}