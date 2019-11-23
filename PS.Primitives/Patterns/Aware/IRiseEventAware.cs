namespace PS.Patterns.Aware
{
    public interface IRiseEventAware<in T>
    {
        #region Members

        void Raise(object sender, T args);

        #endregion
    }
}