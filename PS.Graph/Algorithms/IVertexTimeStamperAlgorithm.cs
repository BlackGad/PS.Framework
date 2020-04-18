namespace PS.Graph.Algorithms
{
    public interface IVertexTimeStamperAlgorithm<out TVertex>
    {
        #region Events

        event VertexAction<TVertex> DiscoverVertex;
        event VertexAction<TVertex> FinishVertex;

        #endregion
    }
}