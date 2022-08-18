namespace PS.Patterns.Aware
{
    public interface IActivationAware : IIsActiveAware
    {
        void Activate();

        void Deactivate();
    }
}
