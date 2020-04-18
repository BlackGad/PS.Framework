using System;
using System.Collections.Generic;
using System.Linq;
using PS.Graph.Collections;

namespace PS.Graph.Algorithms.Exploration
{
    public sealed class TransitionFactoryImplicitGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        private readonly List<ITransitionFactory<TVertex, TEdge>> _transitionFactories;
        private readonly VertexEdgeDictionary<TVertex, TEdge> _vertexEdges;

        #region Constructors

        public TransitionFactoryImplicitGraph()
        {
            _transitionFactories = new List<ITransitionFactory<TVertex, TEdge>>();
            _vertexEdges = new VertexEdgeDictionary<TVertex, TEdge>();
            SuccessorEdgePredicate = e => true;
            SuccessorVertexPredicate = v => true;
        }

        #endregion

        #region Properties

        public EdgePredicate<TVertex, TEdge> SuccessorEdgePredicate { get; set; }

        public VertexPredicate<TVertex> SuccessorVertexPredicate { get; set; }

        public IList<ITransitionFactory<TVertex, TEdge>> TransitionFactories
        {
            get { return _transitionFactories; }
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge> Members

        public bool IsOutEdgesEmpty(TVertex v)
        {
            return OutDegree(v) == 0;
        }

        public int OutDegree(TVertex v)
        {
            return OutEdges(v).Count();
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return _vertexEdges.ContainsKey(vertex);
        }

        public IEnumerable<TEdge> OutEdges(TVertex v)
        {
            if (!_vertexEdges.TryGetValue(v, out var edges))
            {
                edges = new EdgeList<TVertex, TEdge>();
                foreach (var transitionFactory in TransitionFactories)
                {
                    if (!transitionFactory.IsValid(v)) continue;

                    foreach (var edge in transitionFactory.Apply(v))
                    {
                        if (SuccessorVertexPredicate(edge.Target) && SuccessorEdgePredicate(edge))
                        {
                            edges.Add(edge);
                        }
                    }
                }

                _vertexEdges[v] = edges;
            }

            return edges;
        }

        public bool TryGetOutEdges(TVertex v, out IEnumerable<TEdge> edges)
        {
            edges = OutEdges(v);
            return true;
        }

        public TEdge OutEdge(TVertex v, int index)
        {
            var result = OutEdges(v).Skip(index).FirstOrDefault();
            if (result == null) throw new ArgumentOutOfRangeException(nameof(index));
            return result;
        }

        public bool IsDirected
        {
            get { return true; }
        }

        public bool AllowParallelEdges
        {
            get { return true; }
        }

        #endregion
    }
}