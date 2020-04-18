using System;
using System.IO;
using PS.Graph.Algorithms.Condensation;

namespace PS.Graph.Graphviz
{
    public class CondensatedGraphRenderer<TVertex, TEdge, TGraph> : GraphRendererBase<TGraph, CondensedEdge<TVertex, TEdge, TGraph>>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
    {
        #region Constructors

        public CondensatedGraphRenderer(
            IVertexAndEdgeListGraph<TGraph, CondensedEdge<TVertex, TEdge, TGraph>> visitedGraph)
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

        private void Graphviz_FormatEdge(object sender, FormatEdgeEventArgs<TGraph, CondensedEdge<TVertex, TEdge, TGraph>> e)
        {
            var sw = new StringWriter();
            sw.WriteLine("{0}", e.Edge.Edges.Count);
            foreach (var edge in e.Edge.Edges)
            {
                sw.WriteLine("  {0}", edge);
            }

            e.EdgeFormatter.Label.Value = Graphviz.Escape(sw.ToString());
        }

        private void Graphviz_FormatVertex(Object sender, FormatVertexEventArgs<TGraph> e)
        {
            var sw = new StringWriter();
            sw.WriteLine("{0}-{1}", e.Vertex.VertexCount, e.Vertex.EdgeCount);
            foreach (var v in e.Vertex.Vertices)
            {
                sw.WriteLine("  {0}", v);
            }

            foreach (var edge in e.Vertex.Edges)
            {
                sw.WriteLine("  {0}", edge);
            }

            e.VertexFormatter.Label = Graphviz.Escape(sw.ToString());
        }

        #endregion
    }
}