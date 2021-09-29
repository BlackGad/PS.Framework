using System.ComponentModel;
using PS.Patterns.Aware;

namespace PS.Shell.Infrastructure.ViewModels
{
    public interface IControlViewModel : ITitleAware,
                                         IGroupAware
    {
    }
}