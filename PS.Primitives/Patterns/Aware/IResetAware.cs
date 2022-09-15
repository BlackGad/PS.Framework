namespace PS.Patterns.Aware
{
    public interface IResetAware<in TKey>
    {
        void Reset(TKey key);
    }
}
