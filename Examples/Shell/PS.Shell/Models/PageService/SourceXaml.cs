using System.Collections.Generic;
using System.Collections.ObjectModel;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Models.PageService
{
    class SourceXaml : ISourceXaml
    {
        #region Constructors

        public SourceXaml(string title, string code)
        {
            Title = title;
            Code = code;
            Order = 20;
            Children = new ObservableCollection<ISource>();
        }

        #endregion

        #region ISourceXaml Members

        public string Title { get; }
        public IList<ISource> Children { get; }
        public string Code { get; }
        public int Order { get; }

        #endregion
    }
}