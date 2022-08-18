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
        public CancelableProcessCommandView()
        {
            InitializeComponent();
        }

        public CancelableProcessCommandViewModel ViewModel
        {
            get { return DataContext as CancelableProcessCommandViewModel; }
        }

        private void Execute_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Command.Execute(null);
        }
    }
}
