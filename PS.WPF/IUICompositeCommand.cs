using PS.Patterns.Aware;

namespace PS.WPF
{
    public interface IUICompositeCommand : IUICommand, IChildrenAware
    {
    }
}