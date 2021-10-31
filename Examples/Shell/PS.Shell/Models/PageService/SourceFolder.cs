using System.Collections.Generic;
using System.Collections.ObjectModel;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Models.PageService
{
    class SourceFolder : ISourceFolder
    {
        #region Constructors

        public SourceFolder(string title)
        {
            Title = title;
            Children = new ObservableCollection<ISource>();
            Order = 10;
        }

        #endregion

        #region ISourceFolder Members

        public string Title { get; }
        public IList<ISource> Children { get; }
        public int Order { get; }

        #endregion
    }
}