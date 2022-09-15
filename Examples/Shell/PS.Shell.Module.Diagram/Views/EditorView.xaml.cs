using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Diagram.ViewModels;

namespace PS.Shell.Module.Diagram.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<EditorViewModel>))]
    public partial class EditorView : IView<EditorViewModel>
    {
        public EditorView()
        {
            InitializeComponent();
        }

        public EditorViewModel ViewModel
        {
            get { return DataContext as EditorViewModel; }
        }
    }
}
