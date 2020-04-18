namespace PS.Graph.Algorithms
{
    public interface IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge> : IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Events

        event VertexAction<TVertex> FinishVertex;
        event VertexAction<TVertex> StartVertex;

        #endregion
    }
}