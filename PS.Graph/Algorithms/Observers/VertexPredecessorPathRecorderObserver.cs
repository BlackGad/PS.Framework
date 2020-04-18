using System;
using System.Collections.Generic;

namespace PS.Graph.Algorithms.Observers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="boost" />
    [Serializable]
    public sealed class VertexPredecessorPathRecorderObserver<TVertex, TEdge> : IObserver<IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly List<TVertex> _endPathVertices = new List<TVertex>();

        #region Constructors

        public VertexPredecessorPathRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        public VertexPredecessorPathRecorderObserver(
            IDictionary<TVertex, TEdge> vertexPredecessors)
        {
            VertexPredecessors = vertexPredecessors;
        }

        #endregion

        #region Properties

        public ICollection<TVertex> EndPathVertices
        {
            get { return _endPathVertices; }
        }

        public IDictionary<TVertex, TEdge> VertexPredecessors { get; }

        #endregion

        #region IObserver<IVertexPredecessorRecorderAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(IVertexPredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            algorithm.FinishVertex += FinishVertex;
            return new DisposableAction(
                () =>
                {
                    algorithm.TreeEdge -= TreeEdge;
                    algorithm.FinishVertex -= FinishVertex;
                });
        }

        #endregion

        #region Event handlers

        private void FinishVertex(TVertex v)
        {
            foreach (var edge in VertexPredecessors.Values)
            {
                if (edge.Source.Equals(v))
                {
                    return;
                }
            }

            _endPathVertices.Add(v);
        }

        private void TreeEdge(TEdge e)
        {
            VertexPredecessors[e.Target] = e;
        }

        #endregion

        #region Members

        public IEnumerable<IEnumerable<TEdge>> AllPaths()
        {
            var es = new List<IEnumerable<TEdge>>();
            foreach (var v in EndPathVertices)
            {
                if (VertexPredecessors.TryGetPath(v, out var path))
                {
                    es.Add(path);
                }
            }

            return es;
        }

        #endregion
    }
}