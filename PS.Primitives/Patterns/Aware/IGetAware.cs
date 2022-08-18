namespace PS.Patterns.Aware
{
    public interface IGetAware<in TKey, out TValue>
    {
        TValue Get(TKey key);
    }

    public interface IGetAware<in TKey, in TSubKey, out TValue>
    {
        TValue Get(TKey key, TSubKey subKey);
    }
}
