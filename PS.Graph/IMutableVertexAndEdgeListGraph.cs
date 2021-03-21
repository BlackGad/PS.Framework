﻿namespace PS.Graph
{
    /// <summary>
    ///     A mutable vertex and edge list graph
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IMutableVertexAndEdgeListGraph<TVertex, TEdge> : IMutableVertexListGraph<TVertex, TEdge>,
                                                                      IMutableVertexAndEdgeSet<TVertex, TEdge>,
                                                                      IVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}