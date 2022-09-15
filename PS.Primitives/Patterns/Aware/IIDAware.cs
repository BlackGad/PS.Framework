namespace PS.Patterns.Aware
{
    public interface IIDAware : IIDAware<string>
    {
    }

    public interface IIDAware<out T>
    {
        T Id { get; }
    }
}
