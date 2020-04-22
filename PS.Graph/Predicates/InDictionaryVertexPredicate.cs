﻿using System;
using System.Collections.Generic;

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
            _dictionary = dictionary;
        }

        #endregion

        #region Members

        public bool Test(TVertex v)
        {
            return _dictionary.ContainsKey(v);
        }

        #endregion
    }
}