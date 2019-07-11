namespace PS.Patterns.Aware
{
    public interface IAddAware<in TKey, in TSubKey, in TValue>
    {
        #region Members

        bool Add(TKey key, TSubKey subKey, TValue value);

        #endregion
    }

    public interface IAddAware<in TKey, in TValue>
    {
        #region Members

        bool Add(TKey key, TValue value);

        #endregion
    }

    public interface IAddAware<in T>
    {
        #region Members

        bool Add(T value);

        #endregion
    }
}