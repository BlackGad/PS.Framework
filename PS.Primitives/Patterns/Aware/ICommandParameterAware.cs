namespace PS.Patterns.Aware
{
    public interface ICommandParameterAware
    {
        #region Properties

        object CommandParameter { get; }

        #endregion
    }
}