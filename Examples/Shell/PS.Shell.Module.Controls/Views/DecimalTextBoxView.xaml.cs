using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<DecimalTextBoxViewModel>))]
    public partial class DecimalTextBoxView : IView<DecimalTextBoxViewModel>
    {
        #region Constructors

        public DecimalTextBoxView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<DecimalTextBoxViewModel> Members

        public DecimalTextBoxViewModel ViewModel
        {
            get { return DataContext as DecimalTextBoxViewModel; }
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