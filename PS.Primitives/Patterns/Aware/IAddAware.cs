namespace PS.Patterns.Aware
{
    public interface IAddAware<in TKey, in TSubKey, in TValue>
    {
        bool Add(TKey key, TSubKey subKey, TValue value);
    }

    public interface IAddAware<in TKey, in TValue>
    {
        bool Add(TKey key, TValue value);
    }

    public interface IAddAware<in T>
    {
        bool Add(T value);
    }
}
