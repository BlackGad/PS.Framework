using PS.Graph;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface IConnector : IEdge<INode>,
                                  IDiagramComponent
    {
        #region Properties

        IConnectorGeometry Geometry { get; }

        string SourceId { get; }
        string TargetId { get; }
        IConnectorVisual Visual { get; }

        #endregion
    }
}