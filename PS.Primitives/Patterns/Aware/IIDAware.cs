namespace PS.Patterns.Aware
{
    public interface IIDAware : IIDAware<string>
    {
    }

    public interface IIDAware<out T>
    {
        #region Properties

        T Id { get; }

        #endregion
    }
}