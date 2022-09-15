using System.Collections;
using System.Collections.Generic;

namespace PS.Extensions
{
    public static class FluentCollectionExtensions
    {
        public static TElement Put<T, TElement>(this ICollection<T> list, TElement element)
            where TElement : T
        {
            list.Add(element);
            return element;
        }

        public static ICollection<T> PutRange<T>(this ICollection<T> list, IEnumerable enumerable)
        {
            foreach (var element in enumerable.Enumerate())
            {
                list.Add((T)element);
            }

            return list;
        }
    }
}
