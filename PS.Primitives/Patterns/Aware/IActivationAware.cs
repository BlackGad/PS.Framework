namespace PS.Patterns.Aware
{
    public interface IActivationAware : IIsActiveAware
    {
        #region Members

        void Activate();
        void Deactivate();

        #endregion
    }
}