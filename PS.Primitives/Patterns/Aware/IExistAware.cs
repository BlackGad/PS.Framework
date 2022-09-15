namespace PS.Patterns.Aware
{
    public interface IExistAware<in TKey>
    {
        bool Exist(TKey key);
    }
}
