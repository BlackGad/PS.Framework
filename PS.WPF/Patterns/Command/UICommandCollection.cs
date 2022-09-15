using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PS.WPF.Patterns.Command
{
    public class UICommandCollection : ObservableCollection<IUICommand>
    {
        public UICommandCollection()
        {
        }

        public UICommandCollection(IEnumerable<IUICommand> collection)
            : base(collection)
        {
        }
    }
}
