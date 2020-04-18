using System.Collections.Generic;

namespace PS.Graph
{
    /// <summary>
    ///     A mutable vertex set
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    public interface IMutableVertexSet<TVertex> : IVertexSet<TVertex>
    {
        #region Events

        event VertexAction<TVertex> VertexAdded;

        event VertexAction<TVertex> VertexRemoved;

        #endregion

        #region Members

        bool AddVertex(TVertex v);
        int AddVertexRange(IEnumerable<TVertex> vertices);
        bool RemoveVertex(TVertex v);
        int RemoveVertexIf(VertexPredicate<TVertex> predicate);

        #endregion
    }
}