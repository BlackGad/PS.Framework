namespace PS.Patterns.Aware
{
    public interface IStoreAware<in TKey, in TSubKey, in TValue>
    {
        #region Members

        bool Store(TKey key, TSubKey subKey, TValue value);

        #endregion
    }

    public interface IStoreAware<in TKey, in TValue>
    {
        #region Members

        bool Store(TKey key, TValue value);

        #endregion
    }

    public interface IStoreAware<in T>
    {
        #region Members

        bool Store(T value);

        #endregion
    }
}