namespace PS.Patterns.Aware
{
    public interface IMutableDescriptionAware : IDescriptionAware
    {
        new string Description { get; set; }
    }
}
