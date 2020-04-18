namespace PS.Graph.Algorithms
{
    public interface IVertexPredecessorRecorderAlgorithm<TVertex, out TEdge> : ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Events

        event VertexAction<TVertex> FinishVertex;
        event VertexAction<TVertex> StartVertex;

        #endregion
    }
}