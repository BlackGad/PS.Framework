using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace PS.Graph
{
    /// <summary>
    ///     A reversed edge
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{Source}<-{Target}")]
    public struct SReversedEdge<TVertex, TEdge> : IEdge<TVertex>,
                                                  IEquatable<SReversedEdge<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        public SReversedEdge(TEdge originalEdge)
        {
            Contract.Requires(originalEdge != null);

            OriginalEdge = originalEdge;
        }

        public TEdge OriginalEdge { get; }

        public TVertex Source
        {
            get { return OriginalEdge.Target; }
        }

        public TVertex Target
        {
            get { return OriginalEdge.Source; }
        }

        [Pure]
        public override bool Equals(object obj)
        {
            if (!(obj is SReversedEdge<TVertex, TEdge>))
            {
                return false;
            }

            return Equals((SReversedEdge<TVertex, TEdge>)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return OriginalEdge.GetHashCode() ^ 16777619;
        }

        [Pure]
        public override string ToString()
        {
            return $"R({OriginalEdge})";
        }

        [Pure]
        public bool Equals(SReversedEdge<TVertex, TEdge> other)
        {
            return OriginalEdge.Equals(other.OriginalEdge);
        }
    }
}