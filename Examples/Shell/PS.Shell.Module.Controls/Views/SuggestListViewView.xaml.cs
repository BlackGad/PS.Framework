using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<SuggestListViewViewModel>))]
    public partial class SuggestListViewView : IView<SuggestListViewViewModel>
    {
        public SuggestListViewView()
        {
            InitializeComponent();
        }

        public SuggestListViewViewModel ViewModel
        {
            get { return DataContext as SuggestListViewViewModel; }
        }
    }
}
