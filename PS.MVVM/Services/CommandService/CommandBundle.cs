using System.Collections.ObjectModel;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.MVVM.Services.CommandService
{
    public class CommandBundle : CommandServiceComponent,
                                 IContextAware
    {
        private object _contextualId;

        public CommandBundle()
        {
            Groups = new ObservableCollection<CommandGroup>();
        }

        public ObservableCollection<CommandGroup> Groups { get; }

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
    }
}
