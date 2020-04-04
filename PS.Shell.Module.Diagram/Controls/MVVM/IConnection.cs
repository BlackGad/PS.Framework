using QuickGraph;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface IConnection : IEdge<INode>,
                                   IDiagramComponent
    {
        #region Properties

        IConnectionGeometry Geometry { get; }

        string SourceId { get; }
        string TargetId { get; }
        IConnectionVisual Visual { get; }

        #endregion
    }
}