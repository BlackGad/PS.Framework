namespace PS.Patterns.Aware
{
    public interface IEnableAware : IIsEnabledAware
    {
        void Disable();

        void Enable();
    }
}
