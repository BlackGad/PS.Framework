using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <reference-ref
    ///     idref="boost" />
    [Serializable]
    public sealed class VertexRecorderObserver<TVertex> : IObserver<IVertexTimeStamperAlgorithm<TVertex>>
    {
        private readonly IList<TVertex> _vertices;

        #region Constructors

        public VertexRecorderObserver()
            : this(new List<TVertex>())
        {
        }

        public VertexRecorderObserver(IList<TVertex> vertices)
        {
            Contract.Requires(vertices != null);

            _vertices = vertices;
        }

        #endregion

        #region Properties

        public IEnumerable<TVertex> Vertices
        {
            get { return _vertices; }
        }

        #endregion

        #region IObserver<IVertexTimeStamperAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
        {
            algorithm.DiscoverVertex += algorithm_DiscoverVertex;
            return new DisposableAction(
                () => algorithm.DiscoverVertex -= algorithm_DiscoverVertex
            );
        }

        #endregion

        #region Event handlers

        private void algorithm_DiscoverVertex(TVertex v)
        {
            _vertices.Add(v);
        }

        #endregion
    }
}