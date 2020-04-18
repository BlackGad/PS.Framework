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
    public sealed class EdgeRecorderObserver<TVertex, TEdge> : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public EdgeRecorderObserver()
            : this(new List<TEdge>())
        {
        }

        public EdgeRecorderObserver(IList<TEdge> edges)
        {
            Edges = edges;
        }

        #endregion

        #region Properties

        public IList<TEdge> Edges { get; }

        #endregion

        #region IObserver<ITreeBuilderAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.TreeEdge += RecordEdge;
            return new DisposableAction(
                () => algorithm.TreeEdge -= RecordEdge
            );
        }

        #endregion

        #region Event handlers

        private void RecordEdge(TEdge args)
        {
            Edges.Add(args);
        }

        #endregion
    }
}