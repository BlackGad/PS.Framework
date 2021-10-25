using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<CancelableProcessCommandViewModel>))]
    public partial class CancelableProcessCommandView : IView<CancelableProcessCommandViewModel>
    {
        #region Constructors

        public CancelableProcessCommandView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<CancelableProcessCommandViewModel> Members

        public CancelableProcessCommandViewModel ViewModel
        {
            get { return DataContext as CancelableProcessCommandViewModel; }
        }

        #endregion

        #region Event handlers

        private void Execute_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Command.Execute(null);
        }

        #endregion
    }
}