namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public class Node : INode
    {
        #region Constructors

        public Node(string id, object viewModel)
        {
            Id = id;
            ViewModel = viewModel;
            Geometry = new NodeGeometry();
            Visual = new NodeVisual();
        }

        #endregion

        #region INode Members

        public string Id { get; }
        public INodeGeometry Geometry { get; }
        public object ViewModel { get; }
        public INodeVisual Visual { get; }

        #endregion
    }
}