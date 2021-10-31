using System.Collections.Generic;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Models.PageService
{
    class Example : IExample
    {
        #region Constructors

        public Example(string group, string title, object viewModel, IReadOnlyList<string> log)
        {
            Title = title;
            Group = group;
            ViewModel = viewModel;
            Log = log;
        }

        #endregion

        #region IExample Members

        public string Title { get; }
        public string Group { get; }
        public IReadOnlyList<string> Log { get; }
        public object ViewModel { get; }

        #endregion
    }
}