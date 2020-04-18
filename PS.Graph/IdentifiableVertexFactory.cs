namespace PS.Graph
{
    /// <summary>
    ///     A factory of identifiable vertices.
    /// </summary>
    public delegate TVertex IdentifiableVertexFactory<out TVertex>(string id);
}