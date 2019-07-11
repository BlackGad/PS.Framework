namespace PS.Patterns.Aware
{
    public interface IPayloadAware<T>
    {
        #region Properties

        T Payload { get; set; }

        #endregion
    }
}