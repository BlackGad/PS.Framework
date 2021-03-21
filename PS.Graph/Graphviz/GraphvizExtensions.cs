﻿using System;
using System.IO;
using System.Net;

namespace PS.Graph.Graphviz
{
    /// <summary>
    ///     Helper extensions to render graphs to graphviz
    /// </summary>
    public static class GraphvizExtensions
    {
        #region Static members

        /// <summary>
        ///     Renders a graph to the Graphviz DOT format.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static string ToGraphviz<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            return algorithm.Generate();
        }

        /// <summary>
        ///     Renders a graph to the Graphviz DOT format.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="graph"></param>
        /// <param name="initialization">delegate that initializes the algorithm</param>
        /// <returns></returns>
        public static string ToGraphviz<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> graph, Action<GraphvizAlgorithm<TVertex, TEdge>> initialization)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            initialization(algorithm);
            return algorithm.Generate();
        }

        /// <summary>
        ///     Performs a layout .dot in an SVG (Scalable Vector Graphics) file
        ///     by calling Agl through the http://rise4fun.com/ REST services.
        /// </summary>
        /// <returns>the svg graph</returns>
        public static string ToSvg<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return ToSvg(ToGraphviz(graph));
        }

        /// <summary>
        ///     Performs a layout .dot in an SVG (Scalable Vector Graphics) file
        ///     by calling Agl through the http://rise4fun.com/ REST services.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="initialization"></param>
        /// <returns>the svg graph</returns>
        public static string ToSvg<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> graph, Action<GraphvizAlgorithm<TVertex, TEdge>> initialization)
            where TEdge : IEdge<TVertex>
        {
            return ToSvg(ToGraphviz(graph, initialization));
        }

        /// <summary>
        ///     Performs a layout .dot in an SVG (Scalable Vector Graphics) file
        ///     by calling Agl through the http://rise4fun.com/ REST services.
        /// </summary>
        /// <param name="dot">the dot graph</param>
        /// <returns>the svg graph</returns>
        public static string ToSvg(string dot)
        {
            var request = WebRequest.Create("http://rise4fun.com/rest/ask/Agl/");
            request.Method = "POST";
            // write dot
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(dot);
            }

            // read svg
            var response = request.GetResponse();
            string svg;
            var responseStream = response.GetResponseStream() ?? throw new InvalidOperationException();
            using (var reader = new StreamReader(responseStream))
            {
                svg = reader.ReadToEnd();
            }

            return svg;
        }

        #endregion
    }
}