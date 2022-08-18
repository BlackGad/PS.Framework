namespace PS.Patterns.Aware
{
    public interface IFluentActivationAware<out T> : IIsActiveAware
    {
        T Activate();

        T Deactivate();
    }
}
