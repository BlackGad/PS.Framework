namespace PS.Graph
{
    public delegate void ClusterAction<TVertex, TEdge>(IClusteredAdjacencyGraph<TVertex, TEdge> cluster)
        where TEdge : IEdge<TVertex>;
}