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
    public sealed class UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge> : IObserver<IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public UndirectedVertexPredecessorRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        public UndirectedVertexPredecessorRecorderObserver(
            IDictionary<TVertex, TEdge> vertexPredecessors)
        {
            VertexPredecessors = vertexPredecessors;
        }

        #endregion

        #region Properties

        public IDictionary<TVertex, TEdge> VertexPredecessors { get; }

        #endregion

        #region IObserver<IUndirectedTreeBuilderAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(IUndirectedTreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += TreeEdge;
            return new DisposableAction(
                () => algorithm.TreeEdge -= TreeEdge
            );
        }

        #endregion

        #region Event handlers

        private void TreeEdge(Object sender, UndirectedEdgeEventArgs<TVertex, TEdge> e)
        {
            VertexPredecessors[e.Target] = e.Edge;
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