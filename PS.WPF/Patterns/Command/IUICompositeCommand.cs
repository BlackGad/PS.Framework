using PS.Patterns.Aware;

namespace PS.WPF.Patterns.Command
{
    public interface IUICompositeCommand : IUICommand,
                                           IChildrenAware
    {
    }
}
