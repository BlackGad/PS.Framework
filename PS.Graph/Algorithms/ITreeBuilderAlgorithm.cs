namespace PS.Graph.Algorithms
{
    public interface ITreeBuilderAlgorithm<TVertex, out TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Events

        event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion
    }
}