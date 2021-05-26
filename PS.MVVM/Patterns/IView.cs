namespace PS.MVVM.Patterns
{
    public interface IView
    {
    }

    public interface IView<out T> : IView
        where T : IViewModel
    {
        #region Properties

        T ViewModel { get; }

        #endregion
    }
}