namespace PS.Patterns.Aware
{
    public interface ICreateAware<in TKey, out TValue>
    {
        TValue Create(TKey key);
    }

    public interface ICreateAware<in TParam1, in TParam2, in TParam3, out TValue>
    {
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    public interface ICreateAware<in TParam1, in TParam2, out TValue>
    {
        TValue Create(TParam1 param1, TParam2 param2);
    }
}
