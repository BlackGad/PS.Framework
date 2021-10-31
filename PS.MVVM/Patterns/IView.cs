namespace PS.MVVM.Patterns
{
    public interface IView
    {
    }

    public interface IView<out T> : IView
    {
        #region Properties

        T ViewModel { get; }

        #endregion
    }
}