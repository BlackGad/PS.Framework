namespace PS.Patterns.Aware
{
    public interface IMutableDescriptionAware : IDescriptionAware
    {
        #region Properties

        new string Description { get; set; }

        #endregion
    }
}