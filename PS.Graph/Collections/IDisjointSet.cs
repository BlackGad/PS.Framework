using System.Diagnostics.Contracts;
using PS.Graph.Contracts.Collections;

namespace PS.Graph.Collections
{
    /// <summary>
    ///     A disjoint-set data structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ContractClass(typeof(DisjointSetContract<>))]
    public interface IDisjointSet<T>
    {
        #region Properties

        /// <summary>
        ///     Gets the current number of elements.
        /// </summary>
        int ElementCount { get; }

        /// <summary>
        ///     Gets the current number of sets
        /// </summary>
        int SetCount { get; }

        #endregion

        #region Members

        /// <summary>
        ///     Gets a value indicating if left and right are contained in the same set
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool AreInSameSet(T left, T right);

        /// <summary>
        ///     Gets a value indicating whether the value is in the data structure
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        bool Contains(T value);

        /// <summary>
        ///     Finds the set containing the value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T FindSet(T value);

        /// <summary>
        ///     Creates a new set for the value
        /// </summary>
        /// <param name="value"></param>
        void MakeSet(T value);

        /// <summary>
        ///     Merges the sets from the two values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>true if left and right were unioned, false if they already belong to the same set</returns>
        bool Union(T left, T right);

        #endregion
    }
}