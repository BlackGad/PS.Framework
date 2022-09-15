namespace PS.Patterns.Aware
{
    public interface IRiseEventAware<in T>
    {
        void Raise(object sender, T args);
    }
}
