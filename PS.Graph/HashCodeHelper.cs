using System;

namespace PS.Graph
{
    internal static class HashCodeHelper
    {
        #region Constants

        private const Int32 Fnv1Basis32 = unchecked((int)2166136261);
        private const Int64 Fnv1Basis64 = unchecked((int)14695981039346656037);
        private const Int32 Fnv1Prime32 = 16777619;
        private const Int64 Fnv1Prime64 = 1099511628211;

        #endregion

        #region Static members

        /// <summary>
        ///     Combines two hashcodes in a strong way.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Int32 Combine(Int32 x, Int32 y)
        {
            return Fold(Fold(Fnv1Basis32, x), y);
        }

        /// <summary>
        ///     Combines three hashcodes in a strong way.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Int32 Combine(Int32 x, Int32 y, Int32 z)
        {
            return Fold(Fold(Fold(Fnv1Basis32, x), y), z);
        }

        public static Int32 GetHashCode(Int64 x)
        {
            return Combine((Int32)x, (Int32)((UInt64)x >> 32));
        }

        private static Int32 Fold(Int32 hash, byte value)
        {
            return (hash * Fnv1Prime32) ^ value;
        }

        private static Int32 Fold(Int32 hash, Int32 value)
        {
            return Fold(Fold(Fold(Fold(hash,
                                       (byte)value),
                                  (byte)((UInt32)value >> 8)),
                             (byte)((UInt32)value >> 16)),
                        (byte)((UInt32)value >> 24));
        }

        #endregion
    }
}