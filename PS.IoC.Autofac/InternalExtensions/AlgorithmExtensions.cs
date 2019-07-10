using System;
using System.Collections.Generic;

namespace PS.IoC.InternalExtensions
{
    internal static class AlgorithmExtensions
    {
        #region Static members

        public static IEnumerable<T> Traverse<T>(this T root, Func<T, T> traverseFunc, Func<T, bool> continuePredicate = null)
        {
            if (root == null) yield break;
            continuePredicate = continuePredicate ?? (arg => true);
            yield return root;

            foreach (var element in Traverse(traverseFunc(root), traverseFunc, continuePredicate))
            {
                yield return element;
                if (!continuePredicate(element)) break;
            }
        }

        #endregion
    }
}