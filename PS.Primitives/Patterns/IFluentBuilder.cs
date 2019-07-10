namespace PS.Patterns
{
    public interface IFluentBuilder<out T>
    {
        #region Members

        T Create();

        #endregion
    }
}