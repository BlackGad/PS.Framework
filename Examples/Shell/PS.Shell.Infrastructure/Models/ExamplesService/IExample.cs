using System.Collections.Generic;
using PS.Patterns.Aware;

namespace PS.Shell.Infrastructure.Models.ExamplesService
{
    public interface IExample : IGroupAware,
                                ISource
    {
        IReadOnlyList<string> Log { get; }

        object ViewModel { get; }

        IExample Source<T>(string folder = null);

        IExample XamlPage<T>(string folder = null);
    }
}
