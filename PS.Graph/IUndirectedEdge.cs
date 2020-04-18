namespace PS.Graph
{
    /// <summary>
    ///     An undirected edge.
    /// </summary>
    /// <remarks>
    ///     Invariant: source must be less or equal to target (using the default comparer)
    /// </remarks>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    public interface IUndirectedEdge<out TVertex> : IEdge<TVertex>
    {
    }
}