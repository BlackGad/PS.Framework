using System.Collections.Generic;

namespace PS.Shell.Infrastructure.Models.ExamplesService
{
    public interface IExamplesService : IReadOnlyList<IExample>
    {
        IExample Add<T>(string group, string title);
    }
}
