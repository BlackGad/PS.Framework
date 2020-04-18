namespace PS.Graph.Collections
{
    public interface IQueue<T>
    {
        #region Properties

        int Count { get; }

        #endregion

        #region Members

        bool Contains(T value);
        T Dequeue();
        void Enqueue(T value);
        T Peek();

        T[] ToArray();

        #endregion
    }
}