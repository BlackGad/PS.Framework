namespace PS.MVVM.Patterns
{
    public interface IView
    {
    }

    public interface IView<out T> : IView
    {
        T ViewModel { get; }
    }
}
