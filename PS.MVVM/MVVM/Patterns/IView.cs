namespace PS.MVVM.Patterns
{
    public interface IView
    {
    }

    public interface IView<T> : IView
        where T : IViewModel
    {
        #region Properties

        T ViewModel { get; set; }

        #endregion
    }
}