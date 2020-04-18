using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PS.Graph.Collections
{
    [Serializable]
    public class EdgeEdgeDictionary<TVertex, TEdge> : Dictionary<TEdge, TEdge>,
                                                      ICloneable
        where TEdge : IEdge<TVertex>
    {
        #region Constructors

        public EdgeEdgeDictionary()
        {
        }

        public EdgeEdgeDictionary(int capacity)
            : base(capacity)
        {
        }

        protected EdgeEdgeDictionary(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Members

        public EdgeEdgeDictionary<TVertex, TEdge> Clone()
        {
            var clone = new EdgeEdgeDictionary<TVertex, TEdge>(Count);
            foreach (var kv in this)
            {
                clone.Add(kv.Key, kv.Value);
            }

            return clone;
        }

        #endregion
    }
}