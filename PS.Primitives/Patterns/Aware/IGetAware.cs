namespace PS.Patterns.Aware
{
    public interface IGetAware<in TKey, out TValue>
    {
        #region Members

        TValue Get(TKey key);

        #endregion
    }

    public interface IGetAware<in TKey, in TSubKey, out TValue>
    {
        #region Members

        TValue Get(TKey key, TSubKey subKey);

        #endregion
    }
}