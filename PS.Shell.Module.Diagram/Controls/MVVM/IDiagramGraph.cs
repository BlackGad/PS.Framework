using QuickGraph;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface IDiagramGraph : IBidirectionalGraph<INode, IConnection>
    {
        #region Members

        INode Add(string id, object viewModel);

        IConnection Connect(INode source, string sourceId, INode target, string targetId);

        void Delete(IDiagramComponent component);

        #endregion
    }
}