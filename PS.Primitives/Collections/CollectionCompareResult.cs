using System;
using System.Collections.Generic;

namespace PS.Collections
{
    /// <summary>
    ///     Represents IEnumerable.Compare operation result.
    /// </summary>
    public class CollectionCompareResult<TFirst, TSecond>
    {
        #region Properties

        /// <summary>
        ///     Elements which present in both enumerations.
        /// </summary>
        public IReadOnlyCollection<Tuple<TFirst, TSecond>> PresentInBoth { get; internal set; }

        /// <summary>
        ///     Elements which present only in first enumeration.
        /// </summary>
        public IReadOnlyCollection<TFirst> PresentInFirstOnly { get; internal set; }

        /// <summary>
        ///     Elements which present only in second enumeration.
        /// </summary>
        public IReadOnlyCollection<TSecond> PresentInSecondOnly { get; internal set; }

        #endregion
    }
}