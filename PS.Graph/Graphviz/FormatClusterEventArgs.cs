using System;
using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    /// <summary>
    ///     A clustered graph event argument.
    /// </summary>
    public class FormatClusterEventArgs<TVertex, TEdge> : EventArgs
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public FormatClusterEventArgs(IVertexAndEdgeListGraph<TVertex, TEdge> cluster, GraphvizGraph graphFormat)
        {
            Cluster = cluster ?? throw new ArgumentNullException(nameof(cluster));
            GraphFormat = graphFormat;
        }

        #endregion

        #region Properties

        public IVertexAndEdgeListGraph<TVertex, TEdge> Cluster { get; }

        public GraphvizGraph GraphFormat { get; }

        #endregion
    }
}