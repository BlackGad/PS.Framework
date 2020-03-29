using System;
using System.Collections.Generic;
using System.Linq;
using PS.Collections;

namespace PS.Extensions
{
    public static class CollectionExtensions
    {
        #region Static members

        /// <summary>
        ///     Compares two enumeration and returns detailed comparison result.
        /// </summary>
        /// <param name="first">First enumeration to compare.</param>
        /// <param name="second">Second enumeration to compare.</param>
        /// <returns> Returns CollectionCompareResult instance with detailed comparison report.</returns>
        /// <permission>
        ///     Everyone can access this method.
        /// </permission>
        public static CollectionCompareResult<TFirst, TSecond> Compare<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        {
            return Compare(first, second, null, null);
        }

        /// <summary>
        ///     Compares two enumeration and returns detailed comparison result.
        /// </summary>
        /// <param name="first">First enumeration to compare.</param>
        /// <param name="second">Second enumeration to compare.</param>
        /// <param name="hasher">Hash function to use</param>
        /// <returns> Returns CollectionCompareResult instance with detailed comparison report.</returns>
        /// <permission>
        ///     Everyone can access this method.
        /// </permission>
        public static CollectionCompareResult<TItem, TItem> Compare<TItem>(this IEnumerable<TItem> first, IEnumerable<TItem> second, Func<TItem, int> hasher)
        {
            return Compare(first, second, hasher, hasher);
        }

        /// <summary>
        ///     Compares two enumeration and returns detailed comparison result.
        /// </summary>
        /// <param name="first">First enumeration to compare.</param>
        /// <param name="second">Second enumeration to compare.</param>
        /// <param name="firstHasher">Hash function to use for <paramref name="first" /> collection</param>
        /// <param name="secondHasher">Hash function to use for <paramref name="second" /> collection</param>
        /// <returns> Returns CollectionCompareResult instance with detailed comparison report.</returns>
        /// <permission>
        ///     Everyone can access this method.
        /// </permission>
        public static CollectionCompareResult<TFirst, TSecond> Compare<TFirst, TSecond>(this IEnumerable<TFirst> first,
                                                                                        IEnumerable<TSecond> second,
                                                                                        Func<TFirst, int> firstHasher,
                                                                                        Func<TSecond, int> secondHasher)
        {
            firstHasher = firstHasher ?? (i => i.GetHashCode());
            secondHasher = secondHasher ?? (i => i.GetHashCode());

            var firstSet = first.Enumerate().Distinct().ToLookup(v => firstHasher(v), v => v).ToDictionary(g => g.Key, g => g.First());
            var secondSet = second.Enumerate().Distinct().ToLookup(v => secondHasher(v), v => v).ToDictionary(g => g.Key, g => g.First());
            return new CollectionCompareResult<TFirst, TSecond>
            {
                PresentInFirstOnly = firstSet.Keys.Except(secondSet.Keys).Select(k => firstSet[k]).ToList(),
                PresentInSecondOnly = secondSet.Keys.Except(firstSet.Keys).Select(k => secondSet[k]).ToList(),
                PresentInBoth = firstSet.Keys.Intersect(secondSet.Keys).Select(k => new Tuple<TFirst, TSecond>(firstSet[k], secondSet[k])).ToList()
            };
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var lookup = source.Enumerate().ToLookup(keySelector, arg => arg);
            return lookup.Select(g => g.First());
        }

        public static IEnumerable<T> ExceptBy<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return collection.Except(collection.Where(predicate));
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            foreach (var item in collection.Enumerate())
            {
                action(item);
            }
        }

        /// <summary>
        ///     Returns the maximal element of the given sequence, based on
        ///     the given projection.
        /// </summary>
        /// <remarks>
        ///     If more than one element has the maximal projected value, the first
        ///     one encountered will be returned. This overload uses the default comparer
        ///     for the projected type. This operator uses immediate execution, but
        ///     only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="selector" /> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source" /> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                   Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        ///     Returns the maximal element of the given sequence, based on
        ///     the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        ///     If more than one element has the maximal projected value, the first
        ///     one encountered will be returned. This overload uses the default comparer
        ///     for the projected type. This operator uses immediate execution, but
        ///     only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" />, <paramref name="selector" />
        ///     or <paramref name="comparer" /> is null
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source" /> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                   Func<TSource, TKey> selector,
                                                   IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence was empty");
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }

                return max;
            }
        }

        /// <summary>
        ///     Returns the minimal element of the given sequence, based on
        ///     the given projection.
        /// </summary>
        /// <remarks>
        ///     If more than one element has the minimal projected value, the first
        ///     one encountered will be returned. This overload uses the default comparer
        ///     for the projected type. This operator uses immediate execution, but
        ///     only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="selector" /> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source" /> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                   Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        ///     Returns the minimal element of the given sequence, based on
        ///     the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        ///     If more than one element has the minimal projected value, the first
        ///     one encountered will be returned. This overload uses the default comparer
        ///     for the projected type. This operator uses immediate execution, but
        ///     only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" />, <paramref name="selector" />
        ///     or <paramref name="comparer" /> is null
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source" /> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                   Func<TSource, TKey> selector,
                                                   IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence was empty");
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }

                return min;
            }
        }

        public static IEnumerable<T> UnionWith<T>(this IEnumerable<T> enumerable, params T[] unitedItems)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }

            foreach (var item in unitedItems)
            {
                yield return item;
            }
        }

        #endregion
    }
}