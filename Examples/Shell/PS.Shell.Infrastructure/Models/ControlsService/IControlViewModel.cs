using PS.Patterns.Aware;

namespace PS.Shell.Infrastructure.Models.ControlsService
{
    public interface IControlViewModel : ITitleAware,
                                         IGroupAware
    {
    }
}