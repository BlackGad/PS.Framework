using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<TextBoxGenericViewModel>))]
    public partial class TextBoxGenericView : IView<TextBoxGenericViewModel>
    {
        #region Constructors

        public TextBoxGenericView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<TextBoxViewModel> Members

        public TextBoxGenericViewModel ViewModel
        {
            get { return DataContext as TextBoxGenericViewModel; }
        }

        #endregion

        #region Event handlers

        private void BeginEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Control.BeginEdit();
        }

        private void CancelEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Control.CancelEdit();
        }

        private void EndEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Control.EndEdit();
        }

        #endregion
    }
}