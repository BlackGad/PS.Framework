using System.Collections.Generic;
using PS.Patterns.Aware;

namespace PS.Shell.Infrastructure.Models.ExamplesService
{
    public interface IExample : ITitleAware,
                                IGroupAware
    {
        #region Properties

        IReadOnlyList<string> Log { get; }
        object ViewModel { get; }

        #endregion
    }
}