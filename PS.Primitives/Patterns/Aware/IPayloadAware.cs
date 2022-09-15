namespace PS.Patterns.Aware
{
    public interface IPayloadAware<T>
    {
        T Payload { get; set; }
    }
}
