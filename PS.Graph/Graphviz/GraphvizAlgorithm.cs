using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    /// <summary>
    ///     An algorithm that renders a graph to the Graphviz DOT format.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public class GraphvizAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        #region Constants

        // ReSharper disable once StaticMemberInGenericType
        private static readonly Regex WriteLineReplace = new Regex("\n", RegexOptions.Compiled | RegexOptions.Multiline);

        #endregion

        private readonly Dictionary<TVertex, int> _vertexIds = new Dictionary<TVertex, int>();

        #region Constructors

        public GraphvizAlgorithm(IEdgeListGraph<TVertex, TEdge> g)
            : this(g, GraphvizImageType.Png)
        {
        }

        public GraphvizAlgorithm(
            IEdgeListGraph<TVertex, TEdge> g,
            GraphvizImageType imageType
        )
        {
            ClusterCount = 0;
            VisitedGraph = g;
            ImageType = imageType;
            GraphFormat = new GraphvizGraph();
            CommonVertexFormat = new GraphvizVertex();
            CommonEdgeFormat = new GraphvizEdge();
        }

        #endregion

        #region Properties

        public GraphvizEdge CommonEdgeFormat { get; }

        public GraphvizVertex CommonVertexFormat { get; }

        public GraphvizGraph GraphFormat { get; }

        /// <summary>
        ///     Current image output type
        /// </summary>
        public GraphvizImageType ImageType { get; set; }

        /// <summary>
        ///     Dot output stream.
        /// </summary>
        public StringWriter Output { get; private set; }

        public IEdgeListGraph<TVertex, TEdge> VisitedGraph { get; set; }

        internal int ClusterCount { get; set; }

        #endregion

        #region Events

        public event FormatClusterEventHandler<TVertex, TEdge> FormatCluster;

        public event FormatEdgeAction<TVertex, TEdge> FormatEdge;

        public event FormatVertexEventHandler<TVertex> FormatVertex;

        #endregion

        #region Members

        public string Escape(string value)
        {
            return WriteLineReplace.Replace(value, "\\n");
        }

        public string Generate()
        {
            ClusterCount = 0;
            _vertexIds.Clear();
            Output = new StringWriter();
            // build vertex id map
            var i = 0;
            foreach (var v in VisitedGraph.Vertices)
            {
                _vertexIds.Add(v, i++);
            }

            if (VisitedGraph.IsDirected)
            {
                Output.Write("digraph ");
            }
            else
            {
                Output.Write("graph ");
            }

            Output.Write(GraphFormat.Name);
            Output.WriteLine(" {");

            var gf = GraphFormat.ToDot();
            if (gf.Length > 0)
            {
                Output.WriteLine(gf);
            }

            var vf = CommonVertexFormat.ToDot();
            if (vf.Length > 0)
            {
                Output.WriteLine("node [{0}];", vf);
            }

            var ef = CommonEdgeFormat.ToDot();
            if (ef.Length > 0)
            {
                Output.WriteLine("edge [{0}];", ef);
            }

            // initialize vertex map
            var colors = new Dictionary<TVertex, GraphColor>();
            foreach (var v in VisitedGraph.Vertices)
            {
                colors[v] = GraphColor.White;
            }

            var edgeColors = new Dictionary<TEdge, GraphColor>();
            foreach (var e in VisitedGraph.Edges)
            {
                edgeColors[e] = GraphColor.White;
            }

            if (VisitedGraph is IClusteredGraph)
            {
                WriteClusters(colors, edgeColors, VisitedGraph as IClusteredGraph);
            }

            WriteVertices(colors, VisitedGraph.Vertices);
            WriteEdges(edgeColors, VisitedGraph.Edges);

            Output.WriteLine("}");
            return Output.ToString();
        }

        public string Generate(IDotEngine dot, string outputFileName)
        {
            Generate();
            return dot.Run(ImageType, Output.ToString(), outputFileName);
        }

        internal void WriteClusters(
            IDictionary<TVertex, GraphColor> colors,
            IDictionary<TEdge, GraphColor> edgeColors,
            IClusteredGraph parent
        )
        {
            ++ClusterCount;
            foreach (IVertexAndEdgeListGraph<TVertex, TEdge> g in parent.Clusters)
            {
                Output.Write("subgraph cluster{0}", ClusterCount.ToString());
                Output.WriteLine(" {");
                OnFormatCluster(g);
                if (g is IClusteredGraph graph)
                {
                    WriteClusters(colors, edgeColors, graph);
                }

                if (parent.Collapsed)
                {
                    foreach (var v in g.Vertices)
                    {
                        colors[v] = GraphColor.Black;
                    }

                    foreach (var e in g.Edges)
                    {
                        edgeColors[e] = GraphColor.Black;
                    }
                }
                else
                {
                    WriteVertices(colors, g.Vertices);
                    WriteEdges(edgeColors, g.Edges);
                }

                Output.WriteLine("}");
            }
        }

        private void OnFormatCluster(IVertexAndEdgeListGraph<TVertex, TEdge> cluster)
        {
            if (FormatCluster != null)
            {
                var args =
                    new FormatClusterEventArgs<TVertex, TEdge>(cluster, new GraphvizGraph());
                FormatCluster(this, args);
                var s = args.GraphFormat.ToDot();
                if (s.Length != 0)
                {
                    Output.WriteLine(s);
                }
            }
        }

        private void OnFormatEdge(TEdge e)
        {
            if (FormatEdge != null)
            {
                var ev = new GraphvizEdge();
                FormatEdge(this, new FormatEdgeEventArgs<TVertex, TEdge>(e, ev));
                Output.Write(" {0}", ev.ToDot());
            }
        }

        private void OnFormatVertex(TVertex v)
        {
            Output.Write("{0} ", _vertexIds[v]);
            if (FormatVertex != null)
            {
                var gv = new GraphvizVertex
                {
                    Label = v.ToString()
                };
                FormatVertex(this, new FormatVertexEventArgs<TVertex>(v, gv));

                var s = gv.ToDot();
                if (s.Length != 0)
                {
                    Output.Write("[{0}]", s);
                }
            }

            Output.WriteLine(";");
        }

        private void WriteEdges(
            IDictionary<TEdge, GraphColor> edgeColors,
            IEnumerable<TEdge> edges)
        {
            foreach (var e in edges)
            {
                if (edgeColors[e] != GraphColor.White)
                {
                    continue;
                }

                if (VisitedGraph.IsDirected)
                {
                    Output.Write("{0} -> {1} [",
                                 _vertexIds[e.Source],
                                 _vertexIds[e.Target]
                    );
                }
                else
                {
                    Output.Write("{0} -- {1} [",
                                 _vertexIds[e.Source],
                                 _vertexIds[e.Target]
                    );
                }

                OnFormatEdge(e);
                Output.WriteLine("];");

                edgeColors[e] = GraphColor.Black;
            }
        }

        private void WriteVertices(
            IDictionary<TVertex, GraphColor> colors,
            IEnumerable<TVertex> vertices)
        {
            foreach (var v in vertices)
            {
                if (colors[v] == GraphColor.White)
                {
                    OnFormatVertex(v);
                    colors[v] = GraphColor.Black;
                }
            }
        }

        #endregion
    }
}