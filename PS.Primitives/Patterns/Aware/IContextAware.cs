namespace PS.Patterns.Aware
{
    public interface IContextAware
    {
        #region Properties

        object Context { get; }

        #endregion
    }
}