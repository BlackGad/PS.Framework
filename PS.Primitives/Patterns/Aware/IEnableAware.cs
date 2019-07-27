namespace PS.Patterns.Aware
{
    public interface IEnableAware : IIsEnabledAware
    {
        #region Members

        void Disable();
        void Enable();

        #endregion
    }
}