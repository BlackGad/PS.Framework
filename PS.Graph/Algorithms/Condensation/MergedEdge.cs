using System;
using System.Collections.Generic;

namespace PS.Graph.Algorithms.Condensation
{
    [Serializable]
    public sealed class MergedEdge<TVertex, TEdge> : Edge<TVertex>
        where TEdge : IEdge<TVertex>
    {
        #region Static members

        public static MergedEdge<TVertex, TEdge> Merge(
            MergedEdge<TVertex, TEdge> inEdge,
            MergedEdge<TVertex, TEdge> outEdge
        )
        {
            var newEdge = new MergedEdge<TVertex, TEdge>(
                inEdge.Source,
                outEdge.Target)
            {
                _edges = new List<TEdge>(inEdge.Edges.Count + outEdge.Edges.Count)
            };
            newEdge._edges.AddRange(inEdge.Edges);
            newEdge._edges.AddRange(outEdge._edges);

            return newEdge;
        }

        #endregion

        private List<TEdge> _edges = new List<TEdge>();

        #region Constructors

        public MergedEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        #endregion

        #region Properties

        public IList<TEdge> Edges
        {
            get { return _edges; }
        }

        #endregion
    }
}