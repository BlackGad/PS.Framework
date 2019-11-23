using System.Collections.ObjectModel;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.MVVM.Services.CommandService
{
    public class CommandBundle : CommandServiceComponent,
                                 IContextAware
    {
        private object _contextualId;

        #region Constructors

        public CommandBundle()
        {
            Groups = new ObservableCollection<CommandGroup>();
        }

        #endregion

        #region Properties

        public ObservableCollection<CommandGroup> Groups { get; }

        #endregion

        #region IContextAware Members

        public object Context
        {
            get { return _contextualId; }
            set
            {
                if (_contextualId.AreEqual(value)) return;
                _contextualId = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}