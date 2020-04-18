using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PS.Graph.Contracts.Collections;

namespace PS.Graph.Collections
{
    /// <summary>
    ///     A cloneable list of edges
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [ContractClass(typeof(EdgeListContract<,>))]
    public interface IEdgeList<TVertex, TEdge> : IList<TEdge>,
                                                 ICloneable
        where TEdge : IEdge<TVertex>
    {
        #region Members

        /// <summary>
        ///     Gets a clone of this list
        /// </summary>
        /// <returns></returns>
        new
            IEdgeList<TVertex, TEdge> Clone();

        /// <summary>
        ///     Trims excess allocated space
        /// </summary>
        void TrimExcess();

        #endregion
    }
}