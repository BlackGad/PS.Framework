namespace PS.Graph.Algorithms
{
    public interface IEdgePredecessorRecorderAlgorithm<TVertex, out TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Events

        event EdgeEdgeAction<TVertex, TEdge> DiscoverTreeEdge;
        event EdgeAction<TVertex, TEdge> FinishEdge;

        #endregion
    }
}