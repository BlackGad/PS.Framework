namespace PS.Patterns.Aware
{
    public interface IResetAware<in TKey>
    {
        #region Members

        void Reset(TKey key);

        #endregion
    }
}