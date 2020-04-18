using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using PS.Graph.Algorithms.Services;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.ShortestPath
{
    /// <summary>
    ///     Floyd-Warshall all shortest path algorithm,
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public class FloydWarshallAllShortestPathAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<SEquatableEdge<TVertex>, VertexData> _data;
        private readonly IDistanceRelaxer _distanceRelaxer;
        private readonly Func<TEdge, double> _weights;

        #region Constructors

        public FloydWarshallAllShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
        )
            : base(host, visitedGraph)
        {
            Contract.Requires(weights != null);
            Contract.Requires(distanceRelaxer != null);

            _weights = weights;
            _distanceRelaxer = distanceRelaxer;
            _data = new Dictionary<SEquatableEdge<TVertex>, VertexData>();
        }

        public FloydWarshallAllShortestPathAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer)
            : base(visitedGraph)
        {
            Contract.Requires(weights != null);
            Contract.Requires(distanceRelaxer != null);

            _weights = weights;
            _distanceRelaxer = distanceRelaxer;
            _data = new Dictionary<SEquatableEdge<TVertex>, VertexData>();
        }

        public FloydWarshallAllShortestPathAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights)
            : this(visitedGraph, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            var cancelManager = Services.CancelManager;
            // matrix i,j -> path
            _data.Clear();

            var vertices = (VisitedGraph.Vertices ?? Enumerable.Empty<TVertex>()).ToList();
            var edges = VisitedGraph.Edges;

            // prepare the matrix with initial costs
            // walk each edge and add entry in cost dictionary
            foreach (var edge in edges)
            {
                var ij = edge.ToVertexPair<TVertex, TEdge>();
                var cost = _weights(edge);
                if (!_data.TryGetValue(ij, out var value))
                {
                    _data[ij] = new VertexData(cost, edge);
                }
                else if (cost < value.Distance)
                {
                    _data[ij] = new VertexData(cost, edge);
                }
            }

            if (cancelManager.IsCancelling) return;

            // walk each vertices and make sure cost self-cost 0
            foreach (var v in vertices)
            {
                var e = new SEquatableEdge<TVertex>(v, v);
                _data[e] = new VertexData();
            }

            if (cancelManager.IsCancelling) return;

            // iterate k, i, j
            foreach (var vk in vertices)
            {
                if (cancelManager.IsCancelling) return;
                foreach (var vi in vertices)
                {
                    var ik = new SEquatableEdge<TVertex>(vi, vk);
                    if (_data.TryGetValue(ik, out var pathIk))
                    {
                        foreach (var vj in vertices)
                        {
                            var kj = new SEquatableEdge<TVertex>(vk, vj);

                            if (_data.TryGetValue(kj, out var pathKj))
                            {
                                var combined = _distanceRelaxer.Combine(pathIk.Distance, pathKj.Distance);
                                var ij = new SEquatableEdge<TVertex>(vi, vj);
                                if (_data.TryGetValue(ij, out var pathIj))
                                {
                                    if (_distanceRelaxer.Compare(combined, pathIj.Distance) < 0)
                                    {
                                        _data[ij] = new VertexData(combined, vk);
                                    }
                                }
                                else
                                {
                                    _data[ij] = new VertexData(combined, vk);
                                }
                            }
                        }
                    }
                }
            }

            // check negative cycles
            foreach (var vi in vertices)
            {
                var ii = new SEquatableEdge<TVertex>(vi, vi);
                if (_data.TryGetValue(ii, out var value) &&
                    value.Distance < 0)
                {
                    throw new NegativeCycleGraphException();
                }
            }
        }

        #endregion

        #region Members

        [Conditional("DEBUG")]
        public void Dump(TextWriter writer)
        {
            writer.WriteLine("data:");
            foreach (var kv in _data)
            {
                writer.WriteLine("{0}->{1}: {2}",
                                 kv.Key.Source,
                                 kv.Key.Target,
                                 kv.Value.ToString());
            }
        }

        public bool TryGetDistance(TVertex source, TVertex target, out double cost)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);

            if (_data.TryGetValue(new SEquatableEdge<TVertex>(source, target), out var value))
            {
                cost = value.Distance;
                return true;
            }

            cost = -1;
            return false;
        }

        public bool TryGetPath(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> path)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            
            path = null;

            if (source.Equals(target)) return false;

            #if DEBUG
            var set = new HashSet<TVertex>
            {
                source,
                target
            };
            #endif

            var edges = new EdgeList<TVertex, TEdge>();
            var todo = new Stack<SEquatableEdge<TVertex>>();
            todo.Push(new SEquatableEdge<TVertex>(source, target));
            while (todo.Count > 0)
            {
                var current = todo.Pop();
                Contract.Assert(!current.Source.Equals(current.Target));
                if (_data.TryGetValue(current, out var data))
                {
                    if (data.TryGetEdge(out var edge))
                    {
                        edges.Add(edge);
                    }
                    else
                    {
                        if (data.TryGetPredecessor(out var intermediate))
                        {
                            #if DEBUG && !SILVERLIGHT
                            Contract.Assert(set.Add(intermediate));
                            #endif
                            todo.Push(new SEquatableEdge<TVertex>(intermediate, current.Target));
                            todo.Push(new SEquatableEdge<TVertex>(current.Source, intermediate));
                        }
                        else
                        {
                            Contract.Assert(false);
                            return false;
                        }
                    }
                }
                else
                {
                    // no path found
                    return false;
                }
            }

            Contract.Assert(todo.Count == 0);
            Contract.Assert(edges.Count > 0);
            path = edges.ToArray();
            return true;
        }

        #endregion

        #region Nested type: VertexData

        private readonly struct VertexData
        {
            public readonly double Distance;
            private readonly TEdge _edge;
            private readonly bool _edgeStored;
            private readonly TVertex _predecessor;
            private readonly bool _predecessorStored;

            #region Constructors

            public VertexData(double distance, TEdge edge)
            {
                Contract.Requires(edge != null);
                Distance = distance;
                _predecessor = default;
                _predecessorStored = false;
                _edge = edge;
                _edgeStored = true;
            }

            public VertexData(double distance, TVertex predecessor)
            {
                Contract.Requires(predecessor != null);

                Distance = distance;
                _predecessor = predecessor;
                _predecessorStored = true;
                _edge = default;
                _edgeStored = false;
            }

            #endregion

            #region Override members

            public override string ToString()
            {
                if (_edgeStored)
                {
                    return $"e:{Distance}-{_edge}";
                }

                return $"p:{Distance}-{_predecessor}";
            }

            #endregion

            #region Members

            public bool TryGetEdge(out TEdge edge)
            {
                edge = _edge;
                return _edgeStored;
            }

            public bool TryGetPredecessor(out TVertex predecessor)
            {
                predecessor = _predecessor;
                return _predecessorStored;
            }

            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(!_edgeStored || _edge != null);
                Contract.Invariant(!_predecessorStored || _predecessor != null);
            }

            #endregion
        }

        #endregion
    }
}