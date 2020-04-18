using System;
using System.Collections.Generic;

namespace PS.Graph.Algorithms.Exploration
{
    public interface ITransitionFactory<in TVertex, out TEdge>
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        #region Members

        IEnumerable<TEdge> Apply(TVertex source);
        bool IsValid(TVertex v);

        #endregion
    }
}