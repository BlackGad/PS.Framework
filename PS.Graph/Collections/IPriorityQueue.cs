namespace PS.Graph.Collections
{
    public interface IPriorityQueue<T> : IQueue<T>
    {
        #region Members

        void Update(T value);

        #endregion
    }
}