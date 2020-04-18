using System.Collections.Generic;

namespace PS.Graph
{
    public static class EdgeExtensions
    {
        #region Constants

        public const string DebuggerDisplayEdgeFormatString = "{Source}->{Target}";
        public const string DebuggerDisplayTaggedEdgeFormatString = "{Source}->{Target}:{Tag}";
        public const string DebuggerDisplayTaggedUndirectedEdgeFormatString = "{Source}<->{Target}:{Tag}";
        public const string DebuggerDisplayUndirectedEdgeFormatString = "{Source}<->{Target}";
        public const string EdgeFormatString = "{0}->{1}";
        public const string TaggedEdgeFormatString = "{0}->{1}:{2}";
        public const string TaggedUndirectedEdgeFormatString = "{0}<->{1}:{2}";
        public const string UndirectedEdgeFormatString = "{0}<->{1}";

        #endregion

        #region Static members

        /// <summary>
        ///     Given a source vertex, returns the other vertex in the edge
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edge">must not be a self-edge</param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static TVertex GetOtherVertex<TVertex, TEdge>(this TEdge edge,
                                                             TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            return edge.Source.Equals(vertex) ? edge.Target : edge.Source;
        }

        /// <summary>
        ///     Returns the most efficient comparer for the particular type of TEdge.
        ///     If TEdge implements IUndirectedEdge, then only the (source,target) pair
        ///     has to be compared; if not, (source, target) and (target, source) have to be compared.
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <returns></returns>
        public static EdgeEqualityComparer<TVertex, TEdge> GetUndirectedVertexEquality<TVertex, TEdge>()
            where TEdge : IEdge<TVertex>
        {
            if (typeof(IUndirectedEdge<TVertex>).IsAssignableFrom(typeof(TEdge)))
            {
                return SortedVertexEquality;
            }

            return UndirectedVertexEquality;
        }

        public static bool HasCycles<TVertex, TEdge>(this IEnumerable<TEdge> path)
            where TEdge : IEdge<TVertex>
        {
            var vertices = new Dictionary<TVertex, int>();
            var first = true;
            foreach (var edge in path)
            {
                if (first)
                {
                    if (edge.Source.Equals(edge.Target)) // self-edge
                    {
                        return true;
                    }

                    vertices.Add(edge.Source, 0);
                    vertices.Add(edge.Target, 0);
                    first = false;
                }
                else
                {
                    if (vertices.ContainsKey(edge.Target))
                    {
                        return true;
                    }

                    vertices.Add(edge.Target, 0);
                }
            }

            return false;
        }

        /// <summary>
        ///     Gets a value indicating if <paramref name="vertex" /> is adjacent to <paramref name="edge" />
        ///     (is the source or target).
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edge"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static bool IsAdjacent<TVertex, TEdge>(this TEdge edge,
                                                      TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            //

            return edge.Source.Equals(vertex)
                   || edge.Target.Equals(vertex);
        }

        public static bool IsPath<TVertex, TEdge>(this IEnumerable<TEdge> path)
            where TEdge : IEdge<TVertex>
        {
            var first = true;
            var lastTarget = default(TVertex);
            foreach (var edge in path)
            {
                if (first)
                {
                    lastTarget = edge.Target;
                    first = false;
                }
                else
                {
                    if (!lastTarget.Equals(edge.Source))
                    {
                        return false;
                    }

                    lastTarget = edge.Target;
                }
            }

            return true;
        }

        public static bool IsPathWithoutCycles<TVertex, TEdge>(this IEnumerable<TEdge> path)
            where TEdge : IEdge<TVertex>
        {
            var vertices = new Dictionary<TVertex, int>();
            var first = true;
            var lastTarget = default(TVertex);
            foreach (var edge in path)
            {
                if (first)
                {
                    lastTarget = edge.Target;
                    if (IsSelfEdge<TVertex, TEdge>(edge))
                    {
                        return false;
                    }

                    vertices.Add(edge.Source, 0);
                    vertices.Add(lastTarget, 0);
                    first = false;
                }
                else
                {
                    if (!lastTarget.Equals(edge.Source))
                    {
                        return false;
                    }

                    if (vertices.ContainsKey(edge.Target))
                    {
                        return false;
                    }

                    lastTarget = edge.Target;
                    vertices.Add(edge.Target, 0);
                }
            }

            return true;
        }

        /// <summary>
        ///     Checks that <paramref name="root" /> is a predecessor of <paramref name="vertex" />
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="predecessors"></param>
        /// <param name="root"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static bool IsPredecessor<TVertex, TEdge>(this IDictionary<TVertex, TEdge> predecessors,
                                                         TVertex root,
                                                         TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            var current = vertex;
            if (root.Equals(current))
            {
                return true;
            }

            while (predecessors.TryGetValue(current, out var predecessor))
            {
                var source = GetOtherVertex(predecessor, current);
                if (current.Equals(source))
                {
                    return false;
                }

                if (source.Equals(root))
                {
                    return true;
                }

                current = source;
            }

            return false;
        }

        /// <summary>
        ///     Gets a value indicating if the edge is a self edge.
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edge"></param>
        /// <returns></returns>
        public static bool IsSelfEdge<TVertex, TEdge>(this TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return edge.Source.Equals(edge.Target);
        }

        /// <summary>
        ///     Returns a reversed edge enumeration
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static IEnumerable<SReversedEdge<TVertex, TEdge>> ReverseEdges<TVertex, TEdge>(IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            foreach (var edge in edges)
            {
                yield return new SReversedEdge<TVertex, TEdge>(edge);
            }
        }

        /// <summary>
        ///     Gets a value indicating if the vertices of edge match (source, target)
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edge"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool SortedVertexEquality<TVertex, TEdge>(this TEdge edge,
                                                                TVertex source,
                                                                TVertex target)
            where TEdge : IEdge<TVertex>
        {
            return edge.Source.Equals(source) && edge.Target.Equals(target);
        }

        /// <summary>
        ///     Creates a vertex pair (source, target) from the edge
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edge"></param>
        /// <returns></returns>
        public static SEquatableEdge<TVertex> ToVertexPair<TVertex, TEdge>(this TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return new SEquatableEdge<TVertex>(edge.Source, edge.Target);
        }

        /// <summary>
        ///     Tries to get the predecessor path, if reachable.
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="predecessors"></param>
        /// <param name="v"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetPath<TVertex, TEdge>(this IDictionary<TVertex, TEdge> predecessors,
                                                      TVertex v,
                                                      out IEnumerable<TEdge> result)
            where TEdge : IEdge<TVertex>
        {
            var path = new List<TEdge>();

            var vc = v;
            while (predecessors.TryGetValue(vc, out var edge))
            {
                path.Add(edge);
                vc = GetOtherVertex(edge, vc);
            }

            if (path.Count > 0)
            {
                path.Reverse();
                result = path.ToArray();
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        ///     Gets a value indicating if the vertices of edge match (source, target) or
        ///     (target, source)
        /// </summary>
        /// <typeparam name="TVertex">type of the vertices</typeparam>
        /// <typeparam name="TEdge">type of the edges</typeparam>
        /// <param name="edge"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool UndirectedVertexEquality<TVertex, TEdge>(this TEdge edge,
                                                                    TVertex source,
                                                                    TVertex target)
            where TEdge : IEdge<TVertex>
        {
            return edge.Source.Equals(source) && edge.Target.Equals(target) ||
                   edge.Target.Equals(source) && edge.Source.Equals(target);
        }

        #endregion
    }
}