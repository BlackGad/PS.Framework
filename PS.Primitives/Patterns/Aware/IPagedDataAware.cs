namespace PS.Patterns.Aware
{
    public interface IPagedDataAware
    {
        int? Skip { get; }

        int? Take { get; }
    }
}
