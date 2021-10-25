namespace PS.Patterns.Aware
{
    public interface IMutableTitleAware : ITitleAware
    {
        #region Properties

        new string Title { get; set; }

        #endregion
    }
}