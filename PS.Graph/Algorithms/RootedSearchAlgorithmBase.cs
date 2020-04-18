using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms
{
    public abstract class RootedSearchAlgorithmBase<TVertex, TGraph> : RootedAlgorithmBase<TVertex, TGraph>
    {
        private TVertex _goalVertex;
        private bool _hasGoalVertex;

        #region Constructors

        protected RootedSearchAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph)
            : base(host, visitedGraph)
        {
        }

        #endregion

        #region Events

        public event EventHandler GoalReached;

        public event EventHandler GoalVertexChanged;

        #endregion

        #region Members

        public void ClearGoalVertex()
        {
            _goalVertex = default;
            _hasGoalVertex = false;
        }

        public void Compute(TVertex root, TVertex goal)
        {
            SetGoalVertex(goal);
            Compute(root);
        }

        public void SetGoalVertex(TVertex goalVertex)
        {
            var changed = Comparer<TVertex>.Default.Compare(_goalVertex, goalVertex) != 0;
            _goalVertex = goalVertex;
            if (changed)
            {
                OnGoalVertexChanged(EventArgs.Empty);
            }

            _hasGoalVertex = true;
        }

        public bool TryGetGoalVertex(out TVertex goalVertex)
        {
            if (_hasGoalVertex)
            {
                goalVertex = _goalVertex;
                return true;
            }

            goalVertex = default;
            return false;
        }

        protected virtual void OnGoalReached()
        {
            var eh = GoalReached;
            eh?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnGoalVertexChanged(EventArgs e)
        {
            var eh = GoalVertexChanged;
            eh?.Invoke(this, e);
        }

        #endregion
    }
}