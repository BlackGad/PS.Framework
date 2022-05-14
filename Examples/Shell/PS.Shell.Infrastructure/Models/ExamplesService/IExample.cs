using System.Collections.Generic;
using PS.Patterns.Aware;

namespace PS.Shell.Infrastructure.Models.ExamplesService
{
    public interface IExample : IGroupAware,
                                ISource
    {
        #region Properties

        IReadOnlyList<string> Log { get; }
        object ViewModel { get; }

        #endregion

        #region Members

        IExample Source<T>(string folder = null);
        IExample XamlPage<T>(string folder = null);

        #endregion
    }
}