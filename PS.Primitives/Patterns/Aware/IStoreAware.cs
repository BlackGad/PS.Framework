namespace PS.Patterns.Aware
{
    public interface IStoreAware<in TKey, in TSubKey, in TValue>
    {
        bool Store(TKey key, TSubKey subKey, TValue value);
    }

    public interface IStoreAware<in TKey, in TValue>
    {
        bool Store(TKey key, TValue value);
    }

    public interface IStoreAware<in T>
    {
        bool Store(T value);
    }
}
