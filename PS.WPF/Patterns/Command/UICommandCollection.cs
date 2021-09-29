using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PS.WPF.Patterns.Command
{
    public class UICommandCollection : ObservableCollection<IUICommand>
    {
        #region Constructors

        public UICommandCollection()
        {
        }

        public UICommandCollection(IEnumerable<IUICommand> collection)
            : base(collection)
        {
        }

        #endregion
    }
}