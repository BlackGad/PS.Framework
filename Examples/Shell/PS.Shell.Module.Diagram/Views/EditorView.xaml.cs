using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Diagram.ViewModels;

namespace PS.Shell.Module.Diagram.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<EditorViewModel>))]
    public partial class EditorView : IView<EditorViewModel>
    {
        #region Constructors

        public EditorView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<EditorViewModel> Members

        public EditorViewModel ViewModel
        {
            get { return DataContext as EditorViewModel; }
        }

        #endregion
    }
}