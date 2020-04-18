using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Algorithms.Observers
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref
    ///     idref="boost" />
    [Serializable]
    public sealed class EdgePredecessorRecorderObserver<TVertex, TEdge> : IObserver<IEdgePredecessorRecorderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public EdgePredecessorRecorderObserver()
            : this(new Dictionary<TEdge, TEdge>(), new List<TEdge>())
        {
        }

        public EdgePredecessorRecorderObserver(
            IDictionary<TEdge, TEdge> edgePredecessors,
            IList<TEdge> endPathEdges
        )
        {
            Contract.Requires(edgePredecessors != null);
            Contract.Requires(endPathEdges != null);

            EdgePredecessors = edgePredecessors;
            EndPathEdges = endPathEdges;
        }

        #endregion

        #region Properties

        public IDictionary<TEdge, TEdge> EdgePredecessors { get; }

        public IList<TEdge> EndPathEdges { get; }

        #endregion

        #region IObserver<IEdgePredecessorRecorderAlgorithm<TVertex,TEdge>> Members

        public IDisposable Attach(IEdgePredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.DiscoverTreeEdge += DiscoverTreeEdge;
            algorithm.FinishEdge += FinishEdge;

            return new DisposableAction(
                () =>
                {
                    algorithm.DiscoverTreeEdge -= DiscoverTreeEdge;
                    algorithm.FinishEdge -= FinishEdge;
                });
        }

        #endregion

        #region Event handlers

        private void DiscoverTreeEdge(TEdge edge, TEdge targetEdge)
        {
            if (!edge.Equals(targetEdge))
            {
                EdgePredecessors[targetEdge] = edge;
            }
        }

        private void FinishEdge(TEdge args)
        {
            foreach (var edge in EdgePredecessors.Values)
            {
                if (edge.Equals(args))
                {
                    return;
                }
            }

            EndPathEdges.Add(args);
        }

        #endregion

        #region Members

        public ICollection<ICollection<TEdge>> AllMergedPaths()
        {
            var es = new List<ICollection<TEdge>>(EndPathEdges.Count);
            IDictionary<TEdge, GraphColor> colors = new Dictionary<TEdge, GraphColor>();

            foreach (var de in EdgePredecessors)
            {
                colors[de.Key] = GraphColor.White;
                colors[de.Value] = GraphColor.White;
            }

            foreach (var e in EndPathEdges)
            {
                es.Add(MergedPath(e, colors));
            }

            return es;
        }

        public ICollection<ICollection<TEdge>> AllPaths()
        {
            IList<ICollection<TEdge>> es = new List<ICollection<TEdge>>();

            foreach (var e in EndPathEdges)
            {
                es.Add(Path(e));
            }

            return es;
        }

        public ICollection<TEdge> MergedPath(TEdge se, IDictionary<TEdge, GraphColor> colors)
        {
            var path = new List<TEdge>();

            var ec = se;
            var c = colors[ec];
            if (c != GraphColor.White)
            {
                return path;
            }

            colors[ec] = GraphColor.Black;

            path.Insert(0, ec);
            while (EdgePredecessors.ContainsKey(ec))
            {
                var e = EdgePredecessors[ec];
                c = colors[e];
                if (c != GraphColor.White)
                {
                    return path;
                }

                colors[e] = GraphColor.Black;

                path.Insert(0, e);
                ec = e;
            }

            return path;
        }

        public ICollection<TEdge> Path(TEdge se)
        {
            var path = new List<TEdge>();

            var ec = se;
            path.Insert(0, ec);
            while (EdgePredecessors.ContainsKey(ec))
            {
                var e = EdgePredecessors[ec];
                path.Insert(0, e);
                ec = e;
            }

            return path;
        }

        #endregion
    }
}