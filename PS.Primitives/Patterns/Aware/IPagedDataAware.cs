namespace PS.Patterns.Aware
{
    public interface IPagedDataAware
    {
        #region Properties

        int? Skip { get; }
        int? Take { get; }

        #endregion
    }
}