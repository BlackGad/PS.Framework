using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PS.Graph.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     Extension methods for populating graph data structures
    /// </summary>
    public static class GraphExtensions
    {
        #region Static members

        /// <summary>
        ///     Converts a jagged array of sources and targets into a graph
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static AdjacencyGraph<TVertex, SEquatableEdge<TVertex>>
            ToAdjacencyGraph<TVertex>(this TVertex[][] edges
            )
        {
            Contract.Requires(edges != null);
            Contract.Requires(edges.Length == 2);
            Contract.Requires(edges[0] != null);
            Contract.Requires(edges[1] != null);
            Contract.Requires(edges[0].Length == edges[1].Length);
            Contract.Ensures(Contract.Result<AdjacencyGraph<TVertex, SEquatableEdge<TVertex>>>() != null);

            var sources = edges[0];
            var targets = edges[1];
            var n = sources.Length;
            var edgePairs = new List<SEquatableEdge<TVertex>>(n);
            for (var i = 0; i < n; ++i)
            {
                edgePairs.Add(new SEquatableEdge<TVertex>(sources[i], targets[i]));
            }

            return ToAdjacencyGraph(edgePairs);
        }

        /// <summary>
        ///     Converts a set of edges into an adjacency graph.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="edges">The edges.</param>
        /// <param name="allowParallelEdges">if set to <c>true</c>, the graph allows parallel edges.</param>
        /// <returns></returns>
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(this IEnumerable<TEdge> edges,
                                                                                      bool allowParallelEdges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(edges != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(edges));

            var g = new AdjacencyGraph<TVertex, TEdge>(allowParallelEdges);
            g.AddVerticesAndEdgeRange(edges);
            return g;
        }

        /// <summary>
        ///     Converts a set of edges into an adjacency graph.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="edges">The edges.</param>
        /// <returns></returns>
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(this IEnumerable<TEdge> edges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(edges != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(edges));

            return ToAdjacencyGraph<TVertex, TEdge>(edges, true);
        }

        /// <summary>
        ///     Converts a set of vertices into an adjacency graph,
        ///     using an edge factory.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="vertices">The vertices.</param>
        /// <param name="outEdgesFactory">The out edges factory.</param>
        /// <param name="allowParallelEdges">if set to <c>true</c>, the graph allows parallel edges.</param>
        /// <returns></returns>
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                      Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
                                                                                      bool allowParallelEdges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(vertices != null);
            Contract.Requires(outEdgesFactory != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(vertices));

            var g = new AdjacencyGraph<TVertex, TEdge>(allowParallelEdges);
            g.AddVertexRange(vertices);
            foreach (var vertex in g.Vertices)
            {
                g.AddEdgeRange(outEdgesFactory(vertex));
            }

            return g;
        }

        /// <summary>
        ///     Converts a set of vertices into an adjacency graph,
        ///     using an edge factory.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="vertices">The vertices.</param>
        /// <param name="outEdgesFactory">The out edges factory.</param>
        /// <returns></returns>
        public static AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                      Func<TVertex, IEnumerable<TEdge>> outEdgesFactory
        )
            where TEdge : IEdge<TVertex>
        {
            return ToAdjacencyGraph(vertices, outEdgesFactory, true);
        }

        /// <summary>
        ///     Converts a sequence of vertex pairs into an adjancency graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <param name="vertexPairs"></param>
        /// <returns></returns>
        public static AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> ToAdjacencyGraph<TVertex>(this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            Contract.Requires(vertexPairs != null);

            var g = new AdjacencyGraph<TVertex, SEquatableEdge<TVertex>>();
            g.AddVerticesAndEdgeRange(vertexPairs);
            return g;
        }

        /// <summary>
        ///     Creates an immutable array adjacency graph from the input graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static ArrayAdjacencyGraph<TVertex, TEdge> ToArrayAdjacencyGraph<TVertex, TEdge>(this IVertexAndEdgeListGraph<TVertex, TEdge> graph
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(graph != null);

            return new ArrayAdjacencyGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        ///     Creates an immutable array bidirectional graph from the input graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static ArrayBidirectionalGraph<TVertex, TEdge> ToArrayBidirectionalGraph<TVertex, TEdge>(this IBidirectionalGraph<TVertex, TEdge> graph
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(graph != null);

            return new ArrayBidirectionalGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        ///     Creates an immutable array undirected graph from the input graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static ArrayUndirectedGraph<TVertex, TEdge> ToArrayUndirectedGraph<TVertex, TEdge>(this IUndirectedGraph<TVertex, TEdge> graph
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(graph != null);

            return new ArrayUndirectedGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        ///     Wraps a adjacency graph (out-edge only) into a bidirectional graph.
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static IBidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(graph != null);

            if (graph is IBidirectionalGraph<TVertex, TEdge> self)
            {
                return self;
            }

            return new BidirectionAdapterGraph<TVertex, TEdge>(graph);
        }

        /// <summary>
        ///     Converts a set of edges into a bidirectional graph.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="edges">The edges.</param>
        /// <param name="allowParallelEdges">if set to <c>true</c>, the graph allows parallel edges.</param>
        /// <returns></returns>
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(this IEnumerable<TEdge> edges,
                                                                                              bool allowParallelEdges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(edges != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(edges));

            var g = new BidirectionalGraph<TVertex, TEdge>(allowParallelEdges);
            g.AddVerticesAndEdgeRange(edges);
            return g;
        }

        /// <summary>
        ///     Converts a set of edges into a bidirectional graph.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="edges">The edges.</param>
        /// <returns></returns>
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(this IEnumerable<TEdge> edges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(edges != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(edges));

            return ToBidirectionalGraph<TVertex, TEdge>(edges, true);
        }

        /// <summary>
        ///     Converts a set of vertices into a bidirectional graph,
        ///     using an edge factory.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="vertices">The vertices.</param>
        /// <param name="outEdgesFactory">The out edges factory.</param>
        /// <param name="allowParallelEdges">if set to <c>true</c>, the graph allows parallel edges.</param>
        /// <returns></returns>
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                              Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
                                                                                              bool allowParallelEdges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(vertices != null);
            Contract.Requires(outEdgesFactory != null);
            Contract.Requires(EnumerableContract.ElementsNotNull(vertices));

            var g = new BidirectionalGraph<TVertex, TEdge>(allowParallelEdges);
            g.AddVertexRange(vertices);
            foreach (var vertex in g.Vertices)
            {
                g.AddEdgeRange(outEdgesFactory(vertex));
            }

            return g;
        }

        /// <summary>
        ///     Converts a set of vertices into a bidirectional graph,
        ///     using an edge factory
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="vertices">The vertices.</param>
        /// <param name="outEdgesFactory">The out edges factory.</param>
        /// <returns></returns>
        public static BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                              Func<TVertex, IEnumerable<TEdge>> outEdgesFactory
        )
            where TEdge : IEdge<TVertex>
        {
            return ToBidirectionalGraph(vertices, outEdgesFactory, true);
        }

        /// <summary>
        ///     Converts a sequence of vertex pairs into an bidirectional graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <param name="vertexPairs"></param>
        /// <returns></returns>
        public static BidirectionalGraph<TVertex, SEquatableEdge<TVertex>> ToBidirectionalGraph<TVertex>(this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            Contract.Requires(vertexPairs != null);

            var g = new BidirectionalGraph<TVertex, SEquatableEdge<TVertex>>();
            g.AddVerticesAndEdgeRange(vertexPairs);
            return g;
        }

        /// <summary>
        ///     Creates an immutable compressed row graph representation of the visited graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="visitedGraph"></param>
        /// <returns></returns>
        public static CompressedSparseRowGraph<TVertex> ToCompressedRowGraph<TVertex, TEdge>(this IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);

            return CompressedSparseRowGraph<TVertex>.FromGraph(visitedGraph);
        }

        /// <summary>
        ///     Creates an instance of DelegateIncidenceGraph.
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="tryGetOutEdges"></param>
        /// <param name="tryGetInEdges"></param>
        /// <returns></returns>
        public static DelegateBidirectionalIncidenceGraph<TVertex, TEdge> ToDelegateBidirectionalIncidenceGraph<TVertex, TEdge>(
            this TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            TryFunc<TVertex, IEnumerable<TEdge>> tryGetInEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(tryGetOutEdges != null);
            Contract.Requires(tryGetInEdges != null);

            return new DelegateBidirectionalIncidenceGraph<TVertex, TEdge>(tryGetOutEdges, tryGetInEdges);
        }

        /// <summary>
        ///     Creates an instance of DelegateIncidenceGraph.
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="tryGetOutEdges"></param>
        /// <returns></returns>
        public static DelegateIncidenceGraph<TVertex, TEdge> ToDelegateIncidenceGraph<TVertex, TEdge>(this TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(tryGetOutEdges != null);
            return new DelegateIncidenceGraph<TVertex, TEdge>(tryGetOutEdges);
        }

        /// <summary>
        ///     Creates an instance of DelegateIncidenceGraph.
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="getOutEdges"></param>
        /// <returns></returns>
        public static DelegateIncidenceGraph<TVertex, TEdge> ToDelegateIncidenceGraph<TVertex, TEdge>(this Func<TVertex, IEnumerable<TEdge>> getOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(getOutEdges != null);

            return ToDelegateIncidenceGraph(ToTryFunc(getOutEdges));
        }

        /// <summary>
        ///     Wraps a dictionary into an undirected list graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge, TValue>(this IDictionary<TVertex, TValue> dictionary)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TValue : IEnumerable<TEdge>
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(dictionary.Values.All(v => v != null));

            return ToDelegateUndirectedGraph(dictionary, kv => kv.Value);
        }

        /// <summary>
        ///     Wraps a dictionary into an undirected graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="keyValueToOutEdges"></param>
        /// <returns></returns>
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge, TValue>(this IDictionary<TVertex, TValue> dictionary,
                                                                                                                       Func<KeyValuePair<TVertex, TValue>,
                                                                                                                           IEnumerable<TEdge>> keyValueToOutEdges
        )
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(keyValueToOutEdges != null);

            return new DelegateVertexAndEdgeListGraph<TVertex, TEdge>(
                dictionary.Keys,
                delegate(TVertex key, out IEnumerable<TEdge> edges)
                {
                    if (dictionary.TryGetValue(key, out var value))
                    {
                        edges = keyValueToOutEdges(new KeyValuePair<TVertex, TValue>(key, value));
                        return true;
                    }

                    edges = null;
                    return false;
                });
        }

        /// <summary>
        ///     Creates an instance of DelegateIncidenceGraph.
        /// </summary>
        /// <param name="vertices"></param>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="tryGetAdjacentEdges"></param>
        /// <returns></returns>
        public static DelegateUndirectedGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                                        TryFunc<TVertex, IEnumerable<TEdge>> tryGetAdjacentEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(vertices != null);
            Contract.Requires(tryGetAdjacentEdges != null);
            Contract.Requires(vertices.All(v => tryGetAdjacentEdges(v, out var edges)));

            return new DelegateUndirectedGraph<TVertex, TEdge>(vertices, tryGetAdjacentEdges, true);
        }

        /// <summary>
        ///     Creates an instance of DelegateIncidenceGraph.
        /// </summary>
        /// <param name="vertices"></param>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="getAdjacentEdges"></param>
        /// <returns></returns>
        public static DelegateUndirectedGraph<TVertex, TEdge> ToDelegateUndirectedGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                                        Func<TVertex, IEnumerable<TEdge>> getAdjacentEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(vertices != null);
            Contract.Requires(getAdjacentEdges != null);

            return ToDelegateUndirectedGraph(
                vertices,
                ToTryFunc(getAdjacentEdges)
            );
        }

        /// <summary>
        ///     Creates an instance of DelegateIncidenceGraph.
        /// </summary>
        /// <param name="vertices"></param>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="tryGetOutEdges"></param>
        /// <returns></returns>
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                                                      TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(vertices != null);
            Contract.Requires(tryGetOutEdges != null);
            Contract.Requires(vertices.All(v => tryGetOutEdges(v, out var edges)));

            return new DelegateVertexAndEdgeListGraph<TVertex, TEdge>(vertices, tryGetOutEdges);
        }

        /// <summary>
        ///     Creates an instance of DelegateIncidenceGraph.
        /// </summary>
        /// <param name="vertices"></param>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="getOutEdges"></param>
        /// <returns></returns>
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToDelegateVertexAndEdgeListGraph<TVertex, TEdge>(this IEnumerable<TVertex> vertices,
                                                                                                                      Func<TVertex, IEnumerable<TEdge>> getOutEdges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(vertices != null);
            Contract.Requires(getOutEdges != null);

            return ToDelegateVertexAndEdgeListGraph(vertices, ToTryFunc(getOutEdges));
        }

        /// <summary>
        ///     Converts a Func that returns a reference type into a TryFunc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TryFunc<T, TResult> ToTryFunc<T, TResult>(Func<T, TResult> func)
            where TResult : class
        {
            Contract.Requires(func != null);

            return (T value, out TResult result) =>
            {
                result = func(value);
                return result != null;
            };
        }

        /// <summary>
        ///     Converts a sequence of edges into an undirected graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static UndirectedGraph<TVertex, TEdge> ToUndirectedGraph<TVertex, TEdge>(this IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(edges != null);
            Contract.Requires(edges.All(e => e != null));

            return ToUndirectedGraph<TVertex, TEdge>(edges, true);
        }

        /// <summary>
        ///     Converts a sequence of edges into an undirected graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edges"></param>
        /// <param name="allowParallelEdges"></param>
        /// <returns></returns>
        public static UndirectedGraph<TVertex, TEdge> ToUndirectedGraph<TVertex, TEdge>(this IEnumerable<TEdge> edges,
                                                                                        bool allowParallelEdges)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(edges != null);
            Contract.Requires(edges.All(e => e != null));

            var g = new UndirectedGraph<TVertex, TEdge>(allowParallelEdges);
            g.AddVerticesAndEdgeRange(edges);
            return g;
        }

        /// <summary>
        ///     Converts a sequence of vertex pairs into an bidirectional graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <param name="vertexPairs"></param>
        /// <returns></returns>
        public static UndirectedGraph<TVertex, SEquatableEdge<TVertex>> ToUndirectedGraph<TVertex>(this IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            Contract.Requires(vertexPairs != null);

            var g = new UndirectedGraph<TVertex, SEquatableEdge<TVertex>>();
            g.AddVerticesAndEdgeRange(vertexPairs);
            return g;
        }

        /// <summary>
        ///     Wraps a dictionary into a vertex and edge list graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToVertexAndEdgeListGraph<TVertex, TEdge, TValue>(this IDictionary<TVertex, TValue> dictionary)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TValue : IEnumerable<TEdge>
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(dictionary.Values.All(v => v != null));

            return ToVertexAndEdgeListGraph(dictionary, kv => kv.Value);
        }

        /// <summary>
        ///     Wraps a dictionary into a vertex and edge list graph
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="keyValueToOutEdges"></param>
        /// <returns></returns>
        public static DelegateVertexAndEdgeListGraph<TVertex, TEdge> ToVertexAndEdgeListGraph<TVertex, TEdge, TValue>(this IDictionary<TVertex, TValue> dictionary,
                                                                                                                      Func<KeyValuePair<TVertex, TValue>,
                                                                                                                          IEnumerable<TEdge>> keyValueToOutEdges
        )
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(keyValueToOutEdges != null);

            return new DelegateVertexAndEdgeListGraph<TVertex, TEdge>(
                dictionary.Keys,
                delegate(TVertex key, out IEnumerable<TEdge> edges)
                {
                    if (dictionary.TryGetValue(key, out var value))
                    {
                        edges = keyValueToOutEdges(new KeyValuePair<TVertex, TValue>(key, value));
                        return true;
                    }

                    edges = null;
                    return false;
                });
        }

        #endregion
    }
}