namespace PS.Patterns.Aware
{
    public interface ICreateAware<in TKey, out TValue>
    {
        #region Members

        TValue Create(TKey key);

        #endregion
    }

    public interface ICreateAware<in TParam1, in TParam2, in TParam3, out TValue>
    {
        #region Members

        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3);

        #endregion
    }

    public interface ICreateAware<in TParam1, in TParam2, out TValue>
    {
        #region Members

        TValue Create(TParam1 param1, TParam2 param2);

        #endregion
    }
}