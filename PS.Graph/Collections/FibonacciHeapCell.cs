using System.Collections.Generic;

namespace PS.Graph.Collections
{
    public sealed class FibonacciHeapCell<TPriority, TValue>
    {
        public FibonacciHeapLinkedList<TPriority, TValue> Children;

        /// <summary>
        ///     Determines the depth of a node
        /// </summary>
        public int Degree;

        /// <summary>
        ///     Determines of a Node has had a child cut from it before
        /// </summary>
        public bool Marked;

        public FibonacciHeapCell<TPriority, TValue> Next;
        public FibonacciHeapCell<TPriority, TValue> Parent;
        public FibonacciHeapCell<TPriority, TValue> Previous;
        public TPriority Priority;
        public bool Removed;
        public TValue Value;

        #region Members

        public KeyValuePair<TPriority, TValue> ToKeyValuePair()
        {
            return new KeyValuePair<TPriority, TValue>(Priority, Value);
        }

        #endregion
    }
}