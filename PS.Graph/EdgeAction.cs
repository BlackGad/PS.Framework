namespace PS.Graph
{
    /// <summary>
    ///     The handler for events involving edges
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    /// <param name="e"></param>
    public delegate void EdgeAction<TVertex, in TEdge>(TEdge e)
        where TEdge : IEdge<TVertex>;
}