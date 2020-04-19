namespace PS.Graph
{
    public interface IClusteredBidirectionalGraph<TVertex, TEdge> : IClusteredGraph<TVertex, TEdge>,
                                                                    IMutableBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}