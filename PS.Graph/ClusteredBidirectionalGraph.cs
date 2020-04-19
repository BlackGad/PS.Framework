using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}, ClusterCount = {ClusterCount}")]
    public class ClusteredBidirectionalGraph<TVertex, TEdge> : BaseClusteredGraph<TVertex, TEdge>,
                                                               IClusteredBidirectionalGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IList<IClusteredGraph<TVertex, TEdge>> _children;
        private IMutableBidirectionalGraph<TVertex, TEdge> _graph;
        private ClusteredBidirectionalGraph<TVertex, TEdge> _parent;

        #region Constructors

        public ClusteredBidirectionalGraph(IMutableBidirectionalGraph<TVertex, TEdge> wrapped)
            : this()
        {
            _graph = wrapped;
        }

        private ClusteredBidirectionalGraph()
        {
            _children = new List<IClusteredGraph<TVertex, TEdge>>();
        }

        private ClusteredBidirectionalGraph(ClusteredBidirectionalGraph<TVertex, TEdge> parent)
            : this()
        {
            _parent = parent;
            _graph = new BidirectionalGraph<TVertex, TEdge>();
        }

        #endregion

        #region Override members

        protected override IClusteredGraph<TVertex, TEdge> CreateChildCluster()
        {
            return new ClusteredBidirectionalGraph<TVertex, TEdge>(this);
        }

        protected override IList<IClusteredGraph<TVertex, TEdge>> GetClusterChildren()
        {
            return _children;
        }

        protected override IMutableVertexAndEdgeListGraph<TVertex, TEdge> GetClusterGraph()
        {
            return _graph;
        }

        protected override IClusteredGraph<TVertex, TEdge> GetClusterParent()
        {
            return _parent;
        }

        #endregion

        #region IClusteredBidirectionalGraph<TVertex,TEdge> Members

        public void ClearEdges(TVertex v)
        {
            ClearInEdges(v);
            ClearOutEdges(v);
        }

        public void ClearInEdges(TVertex v)
        {
            if (_graph.TryGetInEdges(v, out var inEdges))
            {
                foreach (var inEdge in inEdges.ToList())
                {
                    RemoveEdge(inEdge);
                }
            }
        }

        public int Degree(TVertex v)
        {
            return _graph.Degree(v);
        }

        public int InDegree(TVertex v)
        {
            return _graph.InDegree(v);
        }

        public TEdge InEdge(TVertex v, int index)
        {
            return _graph.InEdge(v, index);
        }

        public IEnumerable<TEdge> InEdges(TVertex v)
        {
            return _graph.InEdges(v);
        }

        public bool IsInEdgesEmpty(TVertex v)
        {
            return _graph.IsInEdgesEmpty(v);
        }

        public int RemoveInEdgeIf(TVertex v, EdgePredicate<TVertex, TEdge> edgePredicate)
        {
            var edgeToRemoveCount = _graph.RemoveInEdgeIf(v, edgePredicate);
            _parent?.RemoveInEdgeIf(v, edgePredicate);

            return edgeToRemoveCount;
        }

        public bool TryGetInEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            return _graph.TryGetInEdges(v, out edges);
        }

        #endregion
    }
}