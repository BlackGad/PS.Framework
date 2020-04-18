using System;
using System.IO;
using PS.Graph.Algorithms.Condensation;

namespace PS.Graph.Graphviz
{
    public class EdgeMergeCondensatedGraphRenderer<TVertex, TEdge> : GraphRendererBase<TVertex, MergedEdge<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public EdgeMergeCondensatedGraphRenderer(
            IVertexAndEdgeListGraph<TVertex, MergedEdge<TVertex, TEdge>> visitedGraph)
            : base(visitedGraph)
        {
        }

        #endregion

        #region Override members

        protected override void Initialize()
        {
            base.Initialize();
            Graphviz.FormatVertex += Graphviz_FormatVertex;
            Graphviz.FormatEdge += Graphviz_FormatEdge;
        }

        #endregion

        #region Event handlers

        private void Graphviz_FormatEdge(object sender, FormatEdgeEventArgs<TVertex, MergedEdge<TVertex, TEdge>> e)
        {
            var sw = new StringWriter();
            sw.WriteLine("{0}", e.Edge.Edges.Count);
            foreach (var edge in e.Edge.Edges)
            {
                sw.WriteLine("  {0}", edge);
            }

            e.EdgeFormatter.Label.Value = Graphviz.Escape(sw.ToString());
        }

        private void Graphviz_FormatVertex(Object sender, FormatVertexEventArgs<TVertex> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();
        }

        #endregion
    }
}