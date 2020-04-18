using System;

namespace PS.Graph.Collections
{
    [Serializable]
    public sealed class Queue<T> : System.Collections.Generic.Queue<T>,
                                   IQueue<T>
    {
    }
}