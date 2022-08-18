namespace PS.Patterns.Aware
{
    public interface IDeleteAware<in TKey>
    {
        bool Delete(TKey key);
    }

    public interface IDeleteAware<in TKey, in TSubKey>
    {
        bool Delete(TKey key, TSubKey subKey);
    }
}
