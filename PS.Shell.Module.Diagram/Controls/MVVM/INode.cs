namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface INode : IDiagramComponent
    {
        #region Properties

        INodeGeometry Geometry { get; }
        object ViewModel { get; }
        INodeVisual Visual { get; }

        #endregion
    }
}