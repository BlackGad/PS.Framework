using System.Collections.Generic;
using System.Collections.ObjectModel;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Models.PageService
{
    class SourceCSharp : ISourceCSharp
    {
        #region Constructors

        public SourceCSharp(string title, string code)
        {
            Title = title;
            Code = code;
            Order = 50;
            Children = new ObservableCollection<ISource>();
        }

        #endregion

        #region ISourceCSharp Members

        public string Title { get; }
        public IList<ISource> Children { get; }
        public string Code { get; }
        public int Order { get; }

        #endregion
    }
}