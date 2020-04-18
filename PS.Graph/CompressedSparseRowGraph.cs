using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace PS.Graph
{
    /// <summary>
    ///     Directed graph representation using a Compressed Sparse Row representation
    ///     (http://www.cs.utk.edu/~dongarra/etemplates/node373.html)
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public sealed class CompressedSparseRowGraph<TVertex> : IEdgeSet<TVertex, SEquatableEdge<TVertex>>,
                                                            IVertexListGraph<TVertex, SEquatableEdge<TVertex>>,
                                                            ICloneable

    {
        #region Static members

        public static CompressedSparseRowGraph<TVertex> FromGraph<TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph
        )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);
            Contract.Ensures(Contract.Result<CompressedSparseRowGraph<TVertex>>() != null);

            var outEdgeStartRanges = new Dictionary<TVertex, Range>(visitedGraph.VertexCount);
            var outEdges = new TVertex[visitedGraph.EdgeCount];

            var start = 0;
            var index = 0;
            foreach (var vertex in visitedGraph.Vertices)
            {
                var end = index + visitedGraph.OutDegree(vertex);
                var range = new Range(start, end);
                outEdgeStartRanges.Add(vertex, range);
                foreach (var edge in visitedGraph.OutEdges(vertex))
                {
                    outEdges[index++] = edge.Target;
                }

                Contract.Assert(index == end);
            }

            Contract.Assert(index == outEdges.Length);

            return new CompressedSparseRowGraph<TVertex>(
                outEdgeStartRanges,
                outEdges);
        }

        #endregion

        private readonly TVertex[] _outEdges;

        private readonly Dictionary<TVertex, Range> _outEdgeStartRanges;

        #region Constructors

        private CompressedSparseRowGraph(
            Dictionary<TVertex, Range> outEdgeStartRanges,
            TVertex[] outEdges
        )
        {
            Contract.Requires(outEdgeStartRanges != null);
            Contract.Requires(outEdges != null);

            _outEdgeStartRanges = outEdgeStartRanges;
            _outEdges = outEdges;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IEdgeSet<TVertex,SEquatableEdge<TVertex>> Members

        public int EdgeCount
        {
            get { return _outEdges.Length; }
        }

        public bool IsEdgesEmpty
        {
            get { return _outEdges.Length > 0; }
        }

        public IEnumerable<SEquatableEdge<TVertex>> Edges
        {
            get
            {
                foreach (var kv in _outEdgeStartRanges)
                {
                    var source = kv.Key;
                    var range = kv.Value;
                    for (var i = range.Start; i < range.End; ++i)
                    {
                        var target = _outEdges[i];
                        yield return new SEquatableEdge<TVertex>(source, target);
                    }
                }
            }
        }

        public bool ContainsEdge(SEquatableEdge<TVertex> edge)
        {
            return ContainsEdge(edge.Source, edge.Target);
        }

        #endregion

        #region IVertexListGraph<TVertex,SEquatableEdge<TVertex>> Members

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (_outEdgeStartRanges.TryGetValue(source, out var range))
            {
                for (var i = range.Start; i < range.End; ++i)
                {
                    if (_outEdges[i].Equals(target))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryGetEdges(
            TVertex source,
            TVertex target,
            out IEnumerable<SEquatableEdge<TVertex>> edges)
        {
            if (ContainsEdge(source, target))
            {
                edges = new[] { new SEquatableEdge<TVertex>(source, target) };
                return true;
            }

            edges = null;
            return false;
        }

        public bool TryGetEdge(
            TVertex source,
            TVertex target,
            out SEquatableEdge<TVertex> edge)
        {
            if (ContainsEdge(source, target))
            {
                edge = new SEquatableEdge<TVertex>(source, target);
                return true;
            }

            edge = default;
            return false;
        }

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return _outEdgeStartRanges[v].Length == 0;
        }

        public int OutDegree(TVertex v)
        {
            return _outEdgeStartRanges[v].Length;
        }

        public IEnumerable<SEquatableEdge<TVertex>> OutEdges(TVertex v)
        {
            var range = _outEdgeStartRanges[v];
            for (var i = range.Start; i < range.End; ++i)
            {
                yield return new SEquatableEdge<TVertex>(v, _outEdges[i]);
            }
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<SEquatableEdge<TVertex>> edges)
        {
            if (_outEdgeStartRanges.TryGetValue(v, out var range) &&
                range.Length > 0)
            {
                edges = OutEdges(v);
                return true;
            }

            edges = null;
            return false;
        }

        public SEquatableEdge<TVertex> OutEdge(TVertex v, int index)
        {
            var range = _outEdgeStartRanges[v];
            var targetIndex = range.Start + index;
            Contract.Assert(targetIndex < range.End);
            return new SEquatableEdge<TVertex>(v, _outEdges[targetIndex]);
        }

        public bool IsDirected
        {
            get { return true; }
        }

        public bool AllowParallelEdges
        {
            get { return false; }
        }

        public bool IsVerticesEmpty
        {
            get { return _outEdgeStartRanges.Count > 0; }
        }

        public int VertexCount
        {
            get { return _outEdgeStartRanges.Count; }
        }

        public IEnumerable<TVertex> Vertices
        {
            get { return _outEdgeStartRanges.Keys; }
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return _outEdgeStartRanges.ContainsKey(vertex);
        }

        #endregion

        #region Members

        public CompressedSparseRowGraph<TVertex> Clone()
        {
            var ranges = new Dictionary<TVertex, Range>(_outEdgeStartRanges);
            var edges = (TVertex[])_outEdges.Clone();
            return new CompressedSparseRowGraph<TVertex>(ranges, edges);
        }

        #endregion

        #region Nested type: Range

        [Serializable]
        private struct Range
        {
            public readonly int End;
            public readonly int Start;

            #region Constructors

            public Range(int start, int end)
            {
                Contract.Requires(start >= 0);
                Contract.Requires(start <= end);
                Contract.Ensures(Contract.ValueAtReturn(out this).Start == start);
                Contract.Ensures(Contract.ValueAtReturn(out this).End == end);

                Start = start;
                End = end;
            }

            #endregion

            #region Properties

            public int Length
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return End - Start;
                }
            }

            #endregion
        }

        #endregion
    }
}