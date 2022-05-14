using PS.Patterns.Aware;

namespace PS.Shell.Infrastructure.Models.ExamplesService
{
    public interface ISource : ITitleAware,
                               IOrderAware,
                               IChildrenAware<ISource>
    {
    }
}