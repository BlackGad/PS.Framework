namespace PS.Patterns.Aware
{
    public interface IMutableTitleAware : ITitleAware
    {
        new string Title { get; set; }
    }
}
