using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace PS.Graph.Collections
{
    /// <summary>
    ///     Specifies the order in which a Heap will Dequeue items.
    /// </summary>
    public enum HeapDirection
    {
        /// <summary>
        ///     Items are Dequeued in Increasing order from least to greatest.
        /// </summary>
        Increasing,

        /// <summary>
        ///     Items are Dequeued in Decreasing order, from greatest to least.
        /// </summary>
        Decreasing
    }

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

    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class FibonacciHeap<TPriority, TValue>
        : IEnumerable<KeyValuePair<TPriority, TValue>>
    {
        #region Static members

        private static int Max<T>(IEnumerable<T> values, Func<T, int> converter)
        {
            var max = int.MinValue;
            foreach (var value in values)
            {
                var v = converter(value);
                if (max < v)
                {
                    max = v;
                }
            }

            return max;
        }

        #endregion

        //We use the approach to avoid unnessecary branches
        private readonly Dictionary<int, FibonacciHeapCell<TPriority, TValue>> _degreeToNode;
        private readonly short _directionMultiplier; //Used to control the direction of the heap, set to 1 if the Heap is increasing, -1 if it's decreasing
        private readonly FibonacciHeapLinkedList<TPriority, TValue> _nodes;

        #region Constructors

        public FibonacciHeap()
            : this(HeapDirection.Increasing, Comparer<TPriority>.Default.Compare)
        {
        }

        public FibonacciHeap(HeapDirection direction)
            : this(direction, Comparer<TPriority>.Default.Compare)
        {
        }

        public FibonacciHeap(HeapDirection direction, Func<TPriority, TPriority, int> priorityComparison)
        {
            _nodes = new FibonacciHeapLinkedList<TPriority, TValue>();
            _degreeToNode = new Dictionary<int, FibonacciHeapCell<TPriority, TValue>>();
            _directionMultiplier = (short)(direction == HeapDirection.Increasing ? 1 : -1);
            Direction = direction;
            PriorityComparison = priorityComparison;
            Count = 0;
        }

        #endregion

        #region Properties

        public int Count { get; private set; }
        public HeapDirection Direction { get; }

        public bool IsEmpty
        {
            get { return _nodes.First == null; }
        }

        public Func<TPriority, TPriority, int> PriorityComparison { get; }

        public FibonacciHeapCell<TPriority, TValue> Top { get; private set; }

        #endregion

        #region IEnumerable<KeyValuePair<TPriority,TValue>> Members

        public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
        {
            var tempHeap = new FibonacciHeap<TPriority, TValue>(Direction, PriorityComparison);
            var nodeStack = new Stack<FibonacciHeapCell<TPriority, TValue>>();
            LambdaHelpers.ForEach(_nodes, x => nodeStack.Push(x));
            while (nodeStack.Count > 0)
            {
                var topNode = nodeStack.Peek();
                tempHeap.Enqueue(topNode.Priority, topNode.Value);
                nodeStack.Pop();
                LambdaHelpers.ForEach(topNode.Children, x => nodeStack.Push(x));
            }

            while (!tempHeap.IsEmpty)
            {
                yield return tempHeap.Top.ToKeyValuePair();
                tempHeap.Dequeue();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Members

        public void ChangeKey(FibonacciHeapCell<TPriority, TValue> node, TPriority newKey)
        {
            ChangeKeyInternal(node, newKey, false);
        }

        public void Delete(FibonacciHeapCell<TPriority, TValue> node)
        {
            ChangeKeyInternal(node, default, true);
            Dequeue();
        }

        public KeyValuePair<TPriority, TValue> Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }

            var result = new KeyValuePair<TPriority, TValue>(
                Top.Priority,
                Top.Value);

            _nodes.Remove(Top);
            Top.Next = null;
            Top.Parent = null;
            Top.Previous = null;
            Top.Removed = true;
            if (_degreeToNode.TryGetValue(Top.Degree, out var currentDegreeNode))
            {
                if (currentDegreeNode == Top)
                {
                    _degreeToNode.Remove(Top.Degree);
                }
            }

            Contract.Assert(Top.Children != null);
            foreach (var child in Top.Children)
            {
                child.Parent = null;
            }

            _nodes.MergeLists(Top.Children);
            Top.Children = null;
            Count--;
            UpdateNext();

            return result;
        }

        public string DrawHeap()
        {
            var lines = new List<string>();
            var columnPosition = 0;
            var list = new List<NodeLevel>();
            foreach (var node in _nodes)
            {
                list.Add(new NodeLevel(node, 0));
            }

            list.Reverse();
            var stack = new Stack<NodeLevel>(list);
            while (stack.Count > 0)
            {
                var currentCell = stack.Pop();
                var lineNum = currentCell.Level;
                if (lines.Count <= lineNum)
                {
                    lines.Add(String.Empty);
                }

                var currentLine = lines[lineNum];
                currentLine = currentLine.PadRight(columnPosition, ' ');
                var nodeString = currentCell.Node.Priority + (currentCell.Node.Marked ? "*" : "") + " ";
                currentLine += nodeString;
                if (currentCell.Node.Children?.First != null)
                {
                    var children = new List<FibonacciHeapCell<TPriority, TValue>>(currentCell.Node.Children);
                    children.Reverse();
                    foreach (var child in children)
                    {
                        stack.Push(new NodeLevel(child, currentCell.Level + 1));
                    }
                }
                else
                {
                    columnPosition += nodeString.Length;
                }

                lines[lineNum] = currentLine;
            }

            return String.Join(Environment.NewLine, lines.ToArray());
        }

        public FibonacciHeapCell<TPriority, TValue> Enqueue(TPriority priority, TValue value)
        {
            var newNode =
                new FibonacciHeapCell<TPriority, TValue>
                {
                    Priority = priority,
                    Value = value,
                    Marked = false,
                    Children = new FibonacciHeapLinkedList<TPriority, TValue>(),
                    Degree = 1,
                    Next = null,
                    Previous = null,
                    Parent = null,
                    Removed = false
                };

            //We don't do any book keeping or maintenance of the heap on Enqueue,
            //We just add this node to the end of the list of Heaps, updating the Next if required
            _nodes.AddLast(newNode);
            if (Top == null ||
                PriorityComparison(newNode.Priority, Top.Priority) * _directionMultiplier < 0)
            {
                Top = newNode;
            }

            Count++;
            return newNode;
        }

        public IEnumerable<KeyValuePair<TPriority, TValue>> GetDestructiveEnumerator()
        {
            while (!IsEmpty)
            {
                yield return Top.ToKeyValuePair();
                Dequeue();
            }
        }

        public void Merge(FibonacciHeap<TPriority, TValue> other)
        {
            if (other.Direction != Direction)
            {
                throw new Exception("Error: Heaps must go in the same direction when merging");
            }

            _nodes.MergeLists(other._nodes);
            if (PriorityComparison(other.Top.Priority, Top.Priority) * _directionMultiplier < 0)
            {
                Top = other.Top;
            }

            Count += other.Count;
        }

        private void ChangeKeyInternal(
            FibonacciHeapCell<TPriority, TValue> node,
            TPriority newKey,
            bool deletingNode)
        {
            var delta = Math.Sign(PriorityComparison(node.Priority, newKey));
            if (delta == 0)
            {
                return;
            }

            if (delta == _directionMultiplier || deletingNode)
            {
                //New value is in the same direciton as the heap
                node.Priority = newKey;
                var parentNode = node.Parent;
                if (parentNode != null && (PriorityComparison(newKey, node.Parent.Priority) * _directionMultiplier < 0 || deletingNode))
                {
                    node.Marked = false;
                    parentNode.Children.Remove(node);
                    UpdateNodesDegree(parentNode);
                    node.Parent = null;
                    _nodes.AddLast(node);
                    //This loop is the cascading cut, we continue to cut
                    //ancestors of the node reduced until we hit a root 
                    //or we found an unmarked ancestor
                    while (parentNode.Marked && parentNode.Parent != null)
                    {
                        parentNode.Parent.Children.Remove(parentNode);
                        UpdateNodesDegree(parentNode);
                        parentNode.Marked = false;
                        _nodes.AddLast(parentNode);
                        var currentParent = parentNode;
                        parentNode = parentNode.Parent;
                        currentParent.Parent = null;
                    }

                    if (parentNode.Parent != null)
                    {
                        //We mark this node to note that it's had a child
                        //cut from it before
                        parentNode.Marked = true;
                    }
                }

                //Update next
                if (deletingNode || PriorityComparison(newKey, Top.Priority) * _directionMultiplier < 0)
                {
                    Top = node;
                }
            }
            else
            {
                //New value is in opposite direction of Heap, cut all children violating heap condition
                node.Priority = newKey;
                if (node.Children != null)
                {
                    List<FibonacciHeapCell<TPriority, TValue>> toUpdate = null;
                    foreach (var child in node.Children)
                    {
                        if (PriorityComparison(node.Priority, child.Priority) * _directionMultiplier > 0)
                        {
                            if (toUpdate == null)
                            {
                                toUpdate = new List<FibonacciHeapCell<TPriority, TValue>>();
                            }

                            toUpdate.Add(child);
                        }
                    }

                    if (toUpdate != null)
                    {
                        foreach (var child in toUpdate)
                        {
                            node.Marked = true;
                            node.Children.Remove(child);
                            child.Parent = null;
                            child.Marked = false;
                            _nodes.AddLast(child);
                            UpdateNodesDegree(node);
                        }
                    }
                }

                UpdateNext();
            }
        }

        private void CompressHeap()
        {
            var node = _nodes.First;
            while (node != null)
            {
                var nextNode = node.Next;
                while (_degreeToNode.TryGetValue(node.Degree, out var currentDegreeNode) && currentDegreeNode != node)
                {
                    _degreeToNode.Remove(node.Degree);
                    if (PriorityComparison(currentDegreeNode.Priority, node.Priority) * _directionMultiplier <= 0)
                    {
                        if (node == nextNode)
                        {
                            nextNode = node.Next;
                        }

                        ReduceNodes(currentDegreeNode, node);
                        node = currentDegreeNode;
                    }
                    else
                    {
                        if (currentDegreeNode == nextNode)
                        {
                            nextNode = currentDegreeNode.Next;
                        }

                        ReduceNodes(node, currentDegreeNode);
                    }
                }

                _degreeToNode[node.Degree] = node;
                node = nextNode;
            }
        }

        /// <summary>
        ///     Given two nodes, adds the child node as a child of the parent node
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNode"></param>
        private void ReduceNodes(
            FibonacciHeapCell<TPriority, TValue> parentNode,
            FibonacciHeapCell<TPriority, TValue> childNode)
        {
            _nodes.Remove(childNode);
            parentNode.Children.AddLast(childNode);
            childNode.Parent = parentNode;
            childNode.Marked = false;
            if (parentNode.Degree == childNode.Degree)
            {
                parentNode.Degree += 1;
            }
        }

        /// <summary>
        ///     Updates the Next pointer, maintaining the heap
        ///     by folding duplicate heap degrees into eachother
        ///     Takes O(lg(N)) time amortized
        /// </summary>
        private void UpdateNext()
        {
            CompressHeap();
            var node = _nodes.First;
            Top = _nodes.First;
            while (node != null)
            {
                if (PriorityComparison(node.Priority, Top.Priority) * _directionMultiplier < 0)
                {
                    Top = node;
                }

                node = node.Next;
            }
        }

        /// <summary>
        ///     Updates the degree of a node, cascading to update the degree of the
        ///     parents if nessecary
        /// </summary>
        /// <param name="parentNode"></param>
        private void UpdateNodesDegree(
            FibonacciHeapCell<TPriority, TValue> parentNode)
        {
            var oldDegree = parentNode.Degree;
            parentNode.Degree =
                parentNode.Children.First != null
                    ? Max(parentNode.Children, x => x.Degree) + 1
                    : 1;
            if (oldDegree != parentNode.Degree)
            {
                if (_degreeToNode.TryGetValue(oldDegree, out var degreeMapValue) && degreeMapValue == parentNode)
                {
                    _degreeToNode.Remove(oldDegree);
                }
                else if (parentNode.Parent != null)
                {
                    UpdateNodesDegree(parentNode.Parent);
                }
            }
        }

        #endregion

        #region Nested type: NodeLevel

        //Draws the current heap in a string.  Marked Nodes have a * Next to them

        private readonly struct NodeLevel
        {
            public readonly int Level;
            public readonly FibonacciHeapCell<TPriority, TValue> Node;

            #region Constructors

            public NodeLevel(FibonacciHeapCell<TPriority, TValue> node, int level)
            {
                Node = node;
                Level = level;
            }

            #endregion
        }

        #endregion
    }
}