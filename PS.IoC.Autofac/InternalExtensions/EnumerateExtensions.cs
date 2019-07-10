using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PS.IoC.InternalExtensions
{
    internal static class EnumerateExtensions
    {
        #region Static members

        public static IEnumerable<T> Enumerate<T>(this object @object)
        {
            return @object is IEnumerable enumerable ? enumerable.OfType<T>() : Enumerable.Empty<T>();
        }

        public static IEnumerable<T> Enumerate<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<object> Enumerate(this object @object)
        {
            var enumerable = @object as IEnumerable;
            return enumerable.Enumerate<object>();
        }

        #endregion
    }
}