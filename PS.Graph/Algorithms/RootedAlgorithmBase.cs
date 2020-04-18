using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms
{
    [Serializable]
    public abstract class RootedAlgorithmBase<TVertex, TGraph> : AlgorithmBase<TGraph>
    {
        private bool _hasRootVertex;
        private TVertex _rootVertex;

        #region Constructors

        protected RootedAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph)
            : base(host, visitedGraph)
        {
        }

        #endregion

        #region Events

        public event EventHandler RootVertexChanged;

        #endregion

        #region Members

        public void ClearRootVertex()
        {
            _rootVertex = default;
            _hasRootVertex = false;
        }

        public void Compute(TVertex rootVertex)
        {
            Contract.Requires(rootVertex != null);

            SetRootVertex(rootVertex);
            Compute();
        }

        public void SetRootVertex(TVertex rootVertex)
        {
            Contract.Requires(rootVertex != null);

            var changed = Comparer<TVertex>.Default.Compare(_rootVertex, rootVertex) != 0;
            _rootVertex = rootVertex;
            if (changed)
            {
                OnRootVertexChanged(EventArgs.Empty);
            }

            _hasRootVertex = true;
        }

        public bool TryGetRootVertex(out TVertex rootVertex)
        {
            if (_hasRootVertex)
            {
                rootVertex = _rootVertex;
                return true;
            }

            rootVertex = default;
            return false;
        }

        protected virtual void OnRootVertexChanged(EventArgs e)
        {
            Contract.Requires(e != null);

            var eh = RootVertexChanged;
            eh?.Invoke(this, e);
        }

        #endregion
    }
}