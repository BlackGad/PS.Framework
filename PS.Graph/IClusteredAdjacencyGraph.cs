namespace PS.Graph
{
    public interface IClusteredAdjacencyGraph<TVertex, TEdge> : IClusteredGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}