using System.Collections.Generic;

namespace PS.Graph.Collections
{
    public sealed class FibonacciHeapLinkedList<TPriority, TValue>
        : IEnumerable<FibonacciHeapCell<TPriority, TValue>>
    {
        private FibonacciHeapCell<TPriority, TValue> _last;

        #region Constructors

        internal FibonacciHeapLinkedList()
        {
            First = null;
            _last = null;
        }

        #endregion

        #region Properties

        public FibonacciHeapCell<TPriority, TValue> First { get; private set; }

        #endregion

        #region IEnumerable<FibonacciHeapCell<TPriority,TValue>> Members

        public IEnumerator<FibonacciHeapCell<TPriority, TValue>> GetEnumerator()
        {
            var current = First;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Members

        internal void AddLast(FibonacciHeapCell<TPriority, TValue> node)
        {
            if (_last != null)
            {
                _last.Next = node;
            }

            node.Previous = _last;
            _last = node;
            if (First == null)
            {
                First = node;
            }
        }

        internal void MergeLists(FibonacciHeapLinkedList<TPriority, TValue> list)
        {
            if (list.First != null)
            {
                if (_last != null)
                {
                    _last.Next = list.First;
                }

                list.First.Previous = _last;
                _last = list._last;
                if (First == null)
                {
                    First = list.First;
                }
            }
        }

        internal void Remove(FibonacciHeapCell<TPriority, TValue> node)
        {
            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            else if (First == node)
            {
                First = node.Next;
            }

            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
            else if (_last == node)
            {
                _last = node.Previous;
            }

            node.Next = null;
            node.Previous = null;
        }

        #endregion
    }
}