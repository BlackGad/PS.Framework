using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.MaximumFlow
{
    [Serializable]
    public sealed class ReversedEdgeAugmentorAlgorithm<TVertex, TEdge> : IDisposable
        where TEdge : IEdge<TVertex>
    {
        private IList<TEdge> _augmentedEdges = new List<TEdge>();

        #region Constructors

       

        public ReversedEdgeAugmentorAlgorithm(IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
                                              EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(edgeFactory != null);

            VisitedGraph = visitedGraph;
            EdgeFactory = edgeFactory;
        }

        #endregion

        #region Properties

        public bool Augmented { get; private set; }

        public ICollection<TEdge> AugmentedEdges
        {
            get { return _augmentedEdges; }
        }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; }

        public Dictionary<TEdge, TEdge> ReversedEdges { get; } = new Dictionary<TEdge, TEdge>();

        public IMutableVertexAndEdgeListGraph<TVertex, TEdge> VisitedGraph { get; }

        #endregion

        #region Events

        public event EdgeAction<TVertex, TEdge> ReversedEdgeAdded;

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (Augmented)
            {
                RemoveReversedEdges();
            }
        }

        #endregion

        #region Members

        public void AddReversedEdges()
        {
            if (Augmented)
            {
                throw new InvalidOperationException("Graph already augmented");
            }

            // step 1, find edges that need reversing
            IList<TEdge> notReversedEdges = new List<TEdge>();
            foreach (var edge in VisitedGraph.Edges)
            {
                // if reversed already found, continue
                if (ReversedEdges.ContainsKey(edge))
                {
                    continue;
                }

                var reversedEdge = FindReversedEdge(edge);
                if (reversedEdge != null)
                {
                    // setup edge
                    ReversedEdges[edge] = reversedEdge;
                    // setup reversed if needed
                    if (!ReversedEdges.ContainsKey(reversedEdge))
                    {
                        ReversedEdges[reversedEdge] = edge;
                    }

                    continue;
                }

                // this edge has no reverse
                notReversedEdges.Add(edge);
            }

            // step 2, go over each not reversed edge, add reverse
            foreach (var edge in notReversedEdges)
            {
                if (ReversedEdges.ContainsKey(edge))
                {
                    continue;
                }

                // already been added
                var reversedEdge = FindReversedEdge(edge);
                if (reversedEdge != null)
                {
                    ReversedEdges[edge] = reversedEdge;
                    continue;
                }

                // need to create one
                reversedEdge = EdgeFactory(edge.Target, edge.Source);
                if (!VisitedGraph.AddEdge(reversedEdge))
                {
                    throw new InvalidOperationException("We should not be here");
                }

                _augmentedEdges.Add(reversedEdge);
                ReversedEdges[edge] = reversedEdge;
                ReversedEdges[reversedEdge] = edge;
                OnReservedEdgeAdded(reversedEdge);
            }

            Augmented = true;
        }

        public void RemoveReversedEdges()
        {
            if (!Augmented)
            {
                throw new InvalidOperationException("Graph is not yet augmented");
            }

            foreach (var edge in _augmentedEdges)
            {
                VisitedGraph.RemoveEdge(edge);
            }

            _augmentedEdges.Clear();
            ReversedEdges.Clear();

            Augmented = false;
        }

        private TEdge FindReversedEdge(TEdge edge)
        {
            foreach (var outEdge in VisitedGraph.OutEdges(edge.Target))
            {
                if (outEdge.Target.Equals(edge.Source))
                {
                    return outEdge;
                }
            }

            return default;
        }

        private void OnReservedEdgeAdded(TEdge e)
        {
            var eh = ReversedEdgeAdded;
            eh?.Invoke(e);
        }

        #endregion
    }
}