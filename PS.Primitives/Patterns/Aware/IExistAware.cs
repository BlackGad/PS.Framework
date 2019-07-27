namespace PS.Patterns.Aware
{
    public interface IExistAware<in TKey>
    {
        #region Members

        bool Exist(TKey key);

        #endregion
    }
}