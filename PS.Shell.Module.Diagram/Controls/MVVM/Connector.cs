using System;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public class Connector : IConnector
    {
        #region Constructors

        public Connector(string id, INode source, INode target, string sourceId, string targetId)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            SourceId = sourceId;
            TargetId = targetId;
            Geometry = new ConnectorGeometry();
            Visual = new ConnectorVisual();
        }

        #endregion

        #region IConnector Members

        public INode Source { get; }
        public INode Target { get; }
        public string Id { get; }
        public IConnectorGeometry Geometry { get; }
        public string SourceId { get; }
        public string TargetId { get; }
        public IConnectorVisual Visual { get; }

        #endregion
    }
}