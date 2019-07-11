namespace PS.Patterns.Aware
{
    public interface IDeleteAware<in TKey>
    {
        #region Members

        bool Delete(TKey key);

        #endregion
    }

    public interface IDeleteAware<in TKey, in TSubKey>
    {
        #region Members

        bool Delete(TKey key, TSubKey subKey);

        #endregion
    }
}