using QuickGraph;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface IDiagramGraph : IBidirectionalGraph<INode, IConnector>
    {
        #region Members

        INode Add(string id, object viewModel);

        IConnector Connect(INode source, INode target, string sourceId = null, string targetId = null);

        void Delete(IDiagramComponent component);

        #endregion
    }
}