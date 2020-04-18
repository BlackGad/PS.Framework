using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PS.Graph.Collections
{
    /// <summary>
    ///     A dictionary of vertices to a list of edges
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IVertexEdgeDictionary<TVertex, TEdge> : IDictionary<TVertex, IEdgeList<TVertex, TEdge>>,
                                                             ICloneable,
                                                             ISerializable
        where TEdge : IEdge<TVertex>
    {
        #region Members

        /// <summary>
        ///     Gets a clone of the dictionary. The vertices and edges are not cloned.
        /// </summary>
        /// <returns></returns>
        new
            IVertexEdgeDictionary<TVertex, TEdge> Clone();

        #endregion
    }
}