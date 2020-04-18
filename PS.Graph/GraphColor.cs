using System;

namespace PS.Graph
{
    /// <summary>
    ///     Colors used in vertex coloring algorithms
    /// </summary>
    [Serializable]
    public enum GraphColor : byte
    {
        /// <summary>
        ///     Usually initial color,
        /// </summary>
        White = 0,

        /// <summary>
        ///     Usually intermediate color,
        /// </summary>
        Gray,

        /// <summary>
        ///     Usually finished color
        /// </summary>
        Black
    }
}