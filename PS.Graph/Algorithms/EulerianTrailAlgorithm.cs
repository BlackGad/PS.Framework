using System;
using System.Collections.Generic;
using System.Linq;
using PS.Graph.Algorithms.Observers;
using PS.Graph.Algorithms.Search;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms
{
    [Serializable]
    public sealed class EulerianTrailAlgorithm<TVertex, TEdge> : RootedAlgorithmBase<TVertex, IMutableVertexAndEdgeListGraph<TVertex, TEdge>>,
                                                                 ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Static members

        /// <summary>
        ///     Computes the number of eulerian trail in the graph.
        /// </summary>
        /// <param name="g"></param>
        /// <returns>number of eulerian trails</returns>
        public static int ComputeEulerianPathCount(IVertexAndEdgeListGraph<TVertex, TEdge> g)
        {
            if (g.EdgeCount < g.VertexCount)
            {
                return 0;
            }

            var odd = g.OddVertices().Count;
            if (odd == 0)
            {
                return 1;
            }

            if (odd % 2 != 0)
            {
                return 0;
            }

            return odd / 2;
        }

        #endregion

        private TVertex _currentVertex;
        private List<TEdge> _temporaryCircuit;
        private List<TEdge> _temporaryEdges;

        #region Constructors

        public EulerianTrailAlgorithm(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        /// <summary>
        ///     Construct an eulerian trail builder        ///
        /// </summary>
        /// <param name="host"></param>
        /// <param name="visitedGraph"></param>
        public EulerianTrailAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
            Circuit = new List<TEdge>();
            _temporaryCircuit = new List<TEdge>();
            _currentVertex = default;
            _temporaryEdges = new List<TEdge>();
        }

        #endregion

        #region Properties

        public List<TEdge> Circuit { get; private set; }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> CircuitEdge;

        public event EdgeAction<TVertex, TEdge> VisitEdge;

        #endregion

        #region Override members

        protected override void InternalCompute()
        {
            if (VisitedGraph.VertexCount == 0)
            {
                return;
            }

            if (!TryGetRootVertex(out var rootVertex))
            {
                rootVertex = VisitedGraph.Vertices.First();
            }

            _currentVertex = rootVertex;
            // start search
            Search(_currentVertex);
            if (CircuitAugmentation())
            {
                return; // circuit is found
            }

            do
            {
                if (!Visit())
                {
                    break; // visit edges and build path
                }

                if (CircuitAugmentation())
                {
                    break; // circuit is found
                }
            } while (true);
        }

        #endregion

        #region ITreeBuilderAlgorithm<TVertex,TEdge> Members

        public event EdgeAction<TVertex, TEdge> TreeEdge;

        #endregion

        #region Members

        /// <summary>
        ///     Adds temporary edges to the graph to make all vertex even.
        /// </summary>
        /// <param name="edgeFactory"></param>
        /// <returns></returns>
        public List<TEdge> AddTemporaryEdges(EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            // first gather odd edges.
            var oddVertices = VisitedGraph.OddVertices();

            // check that there are an even number of them
            if (oddVertices.Count % 2 != 0)
            {
                throw new Exception("number of odd vertices in not even!");
            }

            // add temporary edges to create even edges:
            _temporaryEdges = new List<TEdge>();

            while (oddVertices.Count > 0)
            {
                var u = oddVertices[0];
                // find adjacent odd vertex.
                var found = false;
                var foundAdjacent = false;
                foreach (var e in VisitedGraph.OutEdges(u))
                {
                    var v = e.Target;
                    if (!v.Equals(u) && oddVertices.Contains(v))
                    {
                        foundAdjacent = true;
                        // check that v does not have an out-edge towards u
                        var foundBe = false;
                        foreach (var be in VisitedGraph.OutEdges(v))
                        {
                            if (be.Target.Equals(u))
                            {
                                foundBe = true;
                                break;
                            }
                        }

                        if (foundBe)
                        {
                            continue;
                        }

                        // add temporary edge
                        var tempEdge = edgeFactory(v, u);
                        if (!VisitedGraph.AddEdge(tempEdge))
                        {
                            throw new InvalidOperationException();
                        }

                        // add to collection
                        _temporaryEdges.Add(tempEdge);
                        // remove u,v from oddVertices
                        oddVertices.Remove(u);
                        oddVertices.Remove(v);
                        // set u to null
                        found = true;
                        break;
                    }
                }

                if (!foundAdjacent)
                {
                    // pick another vertex
                    if (oddVertices.Count < 2)
                    {
                        throw new Exception("Eulerian trail failure");
                    }

                    var v = oddVertices[1];
                    var tempEdge = edgeFactory(u, v);
                    if (!VisitedGraph.AddEdge(tempEdge))
                    {
                        throw new InvalidOperationException();
                    }

                    // add to collection
                    _temporaryEdges.Add(tempEdge);
                    // remove u,v from oddVertices
                    oddVertices.Remove(u);
                    oddVertices.Remove(v);
                    // set u to null
                    found = true;
                }

                if (!found)
                {
                    oddVertices.Remove(u);
                    oddVertices.Add(u);
                }
            }

            return _temporaryEdges;
        }

        /// <summary>
        ///     Removes temporary edges
        /// </summary>
        public void RemoveTemporaryEdges()
        {
            // remove from graph
            foreach (var e in _temporaryEdges)
            {
                VisitedGraph.RemoveEdge(e);
            }

            _temporaryEdges.Clear();
        }

        /// <summary>
        ///     Computes the set of eulerian trails that traverse the edge set.
        /// </summary>
        /// <remarks>
        ///     This method returns a set of disjoint eulerian trails. This set
        ///     of trails spans the entire set of edges.
        /// </remarks>
        /// <returns>Eulerian trail set</returns>
        public ICollection<ICollection<TEdge>> Trails()
        {
            var trails = new List<ICollection<TEdge>>();

            var trail = new List<TEdge>();
            foreach (var e in Circuit)
            {
                if (_temporaryEdges.Contains(e))
                {
                    // store previous trail and start new one.
                    if (trail.Count != 0)
                    {
                        trails.Add(trail);
                    }

                    // start new trail
                    trail = new List<TEdge>();
                }
                else
                {
                    trail.Add(e);
                }
            }

            if (trail.Count != 0)
            {
                trails.Add(trail);
            }

            return trails;
        }

        /// <summary>
        ///     Computes a set of eulerian trail, starting at <paramref name="s" />
        ///     that spans the entire graph.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method computes a set of eulerian trail starting at <paramref name="s" />
        ///         that spans the entire graph.The algorithm outline is as follows:
        ///     </para>
        ///     <para>
        ///         The algorithms iterates throught the Eulerian circuit of the augmented
        ///         graph (the augmented graph is the graph with additional edges to make
        ///         the number of odd vertices even).
        ///     </para>
        ///     <para>
        ///         If the current edge is not temporary, it is added to the current trail.
        ///     </para>
        ///     <para>
        ///         If the current edge is temporary, the current trail is finished and
        ///         added to the trail collection. The shortest path between the
        ///         start vertex <paramref name="s" /> and the target vertex of the
        ///         temporary edge is then used to start the new trail. This shortest
        ///         path is computed using the BreadthFirstSearchAlgorithm.
        ///     </para>
        /// </remarks>
        /// <param name="s">start vertex</param>
        /// <returns>eulerian trail set, all starting at s</returns>
        /// <exception cref="ArgumentNullException">s is a null reference.</exception>
        /// <exception cref="Exception">Eulerian trail not computed yet.</exception>
        public ICollection<ICollection<TEdge>> Trails(TVertex s)
        {
            if (Circuit.Count == 0)
            {
                throw new InvalidOperationException("Circuit is empty");
            }

            // find the first edge in the circuit.
            int i;
            for (i = 0; i < Circuit.Count; ++i)
            {
                var e = Circuit[i];
                if (_temporaryEdges.Contains(e))
                {
                    continue;
                }

                if (e.Source.Equals(s))
                {
                    break;
                }
            }

            if (i == Circuit.Count)
            {
                throw new Exception("Did not find vertex in eulerian trail?");
            }

            // create collections
            var trails = new List<ICollection<TEdge>>();
            var trail = new List<TEdge>();
            var bfs =
                new BreadthFirstSearchAlgorithm<TVertex, TEdge>(VisitedGraph);
            var vis =
                new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            vis.Attach(bfs);
            bfs.Compute(s);

            // go throught the edges and build the predecessor table.
            var start = i;
            for (; i < Circuit.Count; ++i)
            {
                var e = Circuit[i];
                if (_temporaryEdges.Contains(e))
                {
                    // store previous trail and start new one.
                    if (trail.Count != 0)
                    {
                        trails.Add(trail);
                    }

                    // start new trail
                    // take the shortest path from the start vertex to
                    // the target vertex
                    if (!vis.TryGetPath(e.Target, out var path))
                    {
                        throw new InvalidOperationException();
                    }

                    trail = new List<TEdge>(path);
                }
                else
                {
                    trail.Add(e);
                }
            }

            // starting again on the circuit
            for (i = 0; i < start; ++i)
            {
                var e = Circuit[i];
                if (_temporaryEdges.Contains(e))
                {
                    // store previous trail and start new one.
                    if (trail.Count != 0)
                    {
                        trails.Add(trail);
                    }

                    // start new trail
                    // take the shortest path from the start vertex to
                    // the target vertex
                    if (!vis.TryGetPath(e.Target, out var path))
                    {
                        throw new InvalidOperationException();
                    }

                    trail = new List<TEdge>(path);
                }
                else
                {
                    trail.Add(e);
                }
            }

            // adding the last element
            if (trail.Count != 0)
            {
                trails.Add(trail);
            }

            return trails;
        }

        /// <summary>
        ///     Merges the temporary circuit with the current circuit
        /// </summary>
        /// <returns>true if all the graph edges are in the circuit</returns>
        private bool CircuitAugmentation()
        {
            var newC = new List<TEdge>(Circuit.Count + _temporaryCircuit.Count);
            int i, j;

            // follow C until w is found
            for (i = 0; i < Circuit.Count; ++i)
            {
                var e = Circuit[i];
                if (e.Source.Equals(_currentVertex))
                {
                    break;
                }

                newC.Add(e);
            }

            // follow D until w is found again
            for (j = 0; j < _temporaryCircuit.Count; ++j)
            {
                var e = _temporaryCircuit[j];
                newC.Add(e);
                OnCircuitEdge(e);
                if (e.Target.Equals(_currentVertex))
                {
                    break;
                }
            }

            _temporaryCircuit.Clear();

            // continue C
            for (; i < Circuit.Count; ++i)
            {
                var e = Circuit[i];
                newC.Add(e);
            }

            // set as new circuit
            Circuit = newC;

            // check if contains all edges
            if (Circuit.Count == VisitedGraph.EdgeCount)
            {
                return true;
            }

            return false;
        }

        private bool NotInCircuit(TEdge edge)
        {
            return !Circuit.Contains(edge)
                   && !_temporaryCircuit.Contains(edge);
        }

        private void OnCircuitEdge(TEdge e)
        {
            var eh = CircuitEdge;
            eh?.Invoke(e);
        }

        private void OnTreeEdge(TEdge e)
        {
            var eh = TreeEdge;
            eh?.Invoke(e);
        }

        private void OnVisitEdge(TEdge e)
        {
            var eh = VisitEdge;
            eh?.Invoke(e);
        }

        private bool Search(TVertex u)
        {
            foreach (var e in SelectOutEdgesNotInCircuit(u))
            {
                OnTreeEdge(e);
                var v = e.Target;
                // add edge to temporary path
                _temporaryCircuit.Add(e);
                // e.Target should be equal to CurrentVertex.
                if (e.Target.Equals(_currentVertex))
                {
                    return true;
                }

                // continue search
                if (Search(v))
                {
                    return true;
                }

                _temporaryCircuit.Remove(e);
            }

            // it's a dead end.
            return false;
        }

        private IEnumerable<TEdge> SelectOutEdgesNotInCircuit(TVertex v)
        {
            foreach (var edge in VisitedGraph.OutEdges(v))
            {
                if (NotInCircuit(edge))
                {
                    yield return edge;
                }
            }
        }

        private TEdge SelectSingleOutEdgeNotInCircuit(TVertex v)
        {
            var en = SelectOutEdgesNotInCircuit(v);
            using (var eor = en.GetEnumerator())
            {
                if (eor.MoveNext()) return eor.Current;
                return default;
            }
        }

        /// <summary>
        ///     Looks for a new path to add to the current vertex.
        /// </summary>
        /// <returns>true if found a new path, false otherwise</returns>
        private bool Visit()
        {
            // find a vertex that needs to be visited
            foreach (var e in Circuit)
            {
                var fe = SelectSingleOutEdgeNotInCircuit(e.Source);
                if (fe != null)
                {
                    OnVisitEdge(fe);
                    _currentVertex = e.Source;
                    if (Search(_currentVertex))
                    {
                        return true;
                    }
                }
            }

            // Could not augment circuit
            return false;
        }

        #endregion
    }
}