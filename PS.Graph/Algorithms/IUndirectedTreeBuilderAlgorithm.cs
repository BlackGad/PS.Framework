namespace PS.Graph.Algorithms
{
    public interface IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Events

        event UndirectedEdgeAction<TVertex, TEdge> TreeEdge;

        #endregion
    }
}