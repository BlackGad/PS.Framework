using System.Collections.ObjectModel;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.MVVM.Services.CommandService
{
    public class CommandGroup : CommandServiceComponent,
                                ILargeIconAware,
                                ISmallIconAware
    {
        private object _largeIcon;
        private object _layout;
        private object _smallIcon;

        public CommandGroup()
        {
            Commands = new ObservableCollection<CommandServiceCommand>();
        }

        public ObservableCollection<CommandServiceCommand> Commands { get; }

        public object Layout
        {
            get { return _layout; }
            set
            {
                if (_layout.AreEqual(value)) return;
                _layout = value;
                OnPropertyChanged();
            }
        }

        public object LargeIcon
        {
            get { return _largeIcon; }
            set
            {
                if (_largeIcon.AreEqual(value)) return;
                _largeIcon = value;
                OnPropertyChanged();
            }
        }

        public object SmallIcon
        {
            get { return _smallIcon; }
            set
            {
                if (_smallIcon.AreEqual(value)) return;
                _smallIcon = value;
                OnPropertyChanged();
            }
        }
    }
}
