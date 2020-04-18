using System;
using System.Collections.Generic;

namespace PS.Graph.Collections
{
    internal static class LambdaHelpers
    {
        #region Static members

        /// <summary>
        ///     Performs an action on each item in a list, used to shortcut a "foreach" loop
        /// </summary>
        /// <typeparam name="T">Type contained in List</typeparam>
        /// <param name="collection">List to enumerate over</param>
        /// <param name="action">Lambda Function to be performed on all elements in List</param>
        public static void ForEach<T>(IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static Stack<T> ToStack<T>(IEnumerable<T> collection)
        {
            var newStack = new Stack<T>();
            ForEach(collection, x => newStack.Push(x));
            return newStack;
        }

        /// <summary>
        ///     Performs an action on each item in a list, used to shortcut a "foreach" loop
        /// </summary>
        /// <typeparam name="T">Type contained in List</typeparam>
        /// <param name="collection">List to enumerate over</param>
        /// <param name="action">Lambda Function to be performed on all elements in List</param>
        internal static void ForEach<T>(IList<T> collection, Action<T> action)
        {
            foreach (var t in collection)
            {
                action(t);
            }
        }

        #endregion
    }
}