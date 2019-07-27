namespace PS.Patterns.Aware
{
    public interface IFluentActivationAware<out T> : IIsActiveAware
    {
        #region Members

        T Activate();
        T Deactivate();

        #endregion
    }
}