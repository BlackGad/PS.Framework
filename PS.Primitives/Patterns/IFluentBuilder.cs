namespace PS.Patterns
{
    public interface IFluentBuilder<out T>
    {
        T Create();
    }
}
