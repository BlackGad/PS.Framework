using System.Collections.Generic;

namespace PS.Shell.Infrastructure.Models.ExamplesService
{
    public interface IExamplesService : IReadOnlyList<IExample>
    {
        #region Members

        IExample Add<T>(string group, string title);

        #endregion
    }
}