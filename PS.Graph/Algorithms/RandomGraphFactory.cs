using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms
{
    public static class RandomGraphFactory
    {
        #region Static members

        public static void Create<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory,
            Random rnd,
            int vertexCount,
            int edgeCount,
            bool selfEdges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(g != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
            Contract.Requires(rnd != null);
            Contract.Requires(vertexCount > 0);
            Contract.Requires(edgeCount >= 0);
            Contract.Requires(
                !(!g.AllowParallelEdges && !selfEdges) ||
                edgeCount <= vertexCount * (vertexCount - 1) // directed graph
            );

            var vertices = new TVertex[vertexCount];
            for (var i = 0; i < vertexCount; ++i)
            {
                g.AddVertex(vertices[i] = vertexFactory());
            }

            var j = 0;
            while (j < edgeCount)
            {
                var a = vertices[rnd.Next(vertexCount)];
                TVertex b;
                do
                {
                    b = vertices[rnd.Next(vertexCount)];
                } while (selfEdges == false && a.Equals(b));

                if (g.AddEdge(edgeFactory(a, b)))
                {
                    ++j;
                }
            }
        }

        public static void Create<TVertex, TEdge>(
            IMutableUndirectedGraph<TVertex, TEdge> g,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory,
            Random rnd,
            int vertexCount,
            int edgeCount,
            bool selfEdges
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(g != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
            Contract.Requires(rnd != null);
            Contract.Requires(vertexCount > 0);
            Contract.Requires(edgeCount >= 0);
            Contract.Requires(
                !(!g.AllowParallelEdges && !selfEdges) ||
                edgeCount <= vertexCount * (vertexCount - 1) / 2
            );

            var vertices = new TVertex[vertexCount];
            for (var i = 0; i < vertexCount; ++i)
            {
                g.AddVertex(vertices[i] = vertexFactory());
            }

            var j = 0;
            while (j < edgeCount)
            {
                var a = vertices[rnd.Next(vertexCount)];
                TVertex b;
                do
                {
                    b = vertices[rnd.Next(vertexCount)];
                } while (selfEdges == false && a.Equals(b));

                if (g.AddEdge(edgeFactory(a, b)))
                {
                    ++j;
                }
            }
        }

        public static TEdge GetEdge<TVertex, TEdge>(IEdgeSet<TVertex, TEdge> g, Random rnd)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(g != null);
            Contract.Requires(rnd != null);
            Contract.Requires(g.EdgeCount > 0);

            var i = rnd.Next(g.EdgeCount);
            foreach (var e in g.Edges)
            {
                if (i == 0)
                {
                    return e;
                }

                --i;
            }

            // failed
            throw new InvalidOperationException("Could not find edge");
        }

        public static TEdge GetEdge<TVertex, TEdge>(IEnumerable<TEdge> edges, int count, Random rnd)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(edges != null);
            Contract.Requires(rnd != null);
            Contract.Requires(count > 0);

            var i = rnd.Next(count);
            foreach (var e in edges)
            {
                if (i == 0)
                {
                    return e;
                }

                --i;
            }

            // failed
            throw new InvalidOperationException("Could not find edge");
        }

        public static TVertex GetVertex<TVertex, TEdge>(IVertexListGraph<TVertex, TEdge> g, Random rnd)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(g != null);
            Contract.Requires(rnd != null);
            Contract.Requires(g.VertexCount > 0);

            return GetVertex(g.Vertices, g.VertexCount, rnd);
        }

        public static TVertex GetVertex<TVertex>(IEnumerable<TVertex> vertices, int count, Random rnd)
        {
            Contract.Requires(vertices != null);
            Contract.Requires(rnd != null);
            Contract.Requires(count > 0);

            var i = rnd.Next(count);
            foreach (var v in vertices)
            {
                if (i == 0)
                {
                    return v;
                }

                --i;
            }

            // failed
            throw new InvalidOperationException("Could not find vertex");
        }

        #endregion
    }
}