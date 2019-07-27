namespace PS.Patterns.Aware
{
    public interface IValueAware<out T>
    {
        #region Properties

        T Value { get; }

        #endregion
    }

    public interface IValueAware : IValueAware<object>
    {
    }
}