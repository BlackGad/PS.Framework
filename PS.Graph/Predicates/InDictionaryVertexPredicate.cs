using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace PS.Graph.Predicates
{
    [Serializable]
    public sealed class InDictionaryVertexPredicate<TVertex, TValue>
    {
        private readonly IDictionary<TVertex, TValue> _dictionary;

        #region Constructors

        public InDictionaryVertexPredicate(
            IDictionary<TVertex, TValue> dictionary)
        {
            Contract.Requires(dictionary != null);
            _dictionary = dictionary;
        }

        #endregion

        #region Members

        [Pure]
        public bool Test(TVertex v)
        {
            Contract.Requires(v != null);

            return _dictionary.ContainsKey(v);
        }

        #endregion
    }
}