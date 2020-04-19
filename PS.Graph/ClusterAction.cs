namespace PS.Graph
{
    public delegate void ClusterAction<TVertex, TEdge>(IClusteredGraph<TVertex, TEdge> cluster)
        where TEdge : IEdge<TVertex>;
}