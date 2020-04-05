using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Diagram.ViewModels.Nodes;

namespace PS.Shell.Module.Diagram.Views.Nodes
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<NodeEndViewModel>))]
    public partial class NodeEndView : IView<NodeEndViewModel>
    {
        #region Constructors

        public NodeEndView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public NodeEndViewModel ViewModel
        {
            get { return DataContext as NodeEndViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}