namespace PS.Patterns.Aware
{
    public interface IValueAware<out T>
    {
        T Value { get; }
    }

    public interface IValueAware : IValueAware<object>
    {
    }
}
