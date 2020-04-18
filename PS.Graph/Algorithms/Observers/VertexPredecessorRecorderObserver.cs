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
    public sealed class VertexPredecessorRecorderObserver<TVertex, TEdge> : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, TEdge> _vertexPredecessors;

        #region Constructors

        public VertexPredecessorRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        public VertexPredecessorRecorderObserver(
            Dictionary<TVertex, TEdge> vertexPredecessors)
        {
            _vertexPredecessors = vertexPredecessors;
        }

        #endregion

        #region Properties

        public IDictionary<TVertex, TEdge> VertexPredecessors
        {
            get { return _vertexPredecessors; }
        }

        #endregion

        #region IObserver<ITreeBuilderAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            return new DisposableAction(
                () => algorithm.TreeEdge -= TreeEdge
            );
        }

        #endregion

        #region Event handlers

        private void TreeEdge(TEdge e)
        {
            _vertexPredecessors[e.Target] = e;
        }

        #endregion

        #region Members

        public bool TryGetPath(TVertex vertex, out IEnumerable<TEdge> path)
        {
            return VertexPredecessors.TryGetPath(vertex, out path);
        }

        #endregion
    }
}