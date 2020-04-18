using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PS.Graph.Contracts
{
    public static class EnumerableContract
    {
        #region Static members

        [Pure]
        public static bool All(int lowerBound, int exclusiveUpperBound, Func<int, bool> predicate)
        {
            for (var i = lowerBound; i < exclusiveUpperBound; i++)
            {
                if (!predicate(i)) return false;
            }

            return true;
        }

        [Pure]
        public static bool ElementsNotNull<T>(IEnumerable<T> elements)
        {
            Contract.Requires(elements != null);
            #if DEBUG

            return elements.All(e => e != null);
            #else
            return true;
            #endif
        }

        #endregion
    }
}