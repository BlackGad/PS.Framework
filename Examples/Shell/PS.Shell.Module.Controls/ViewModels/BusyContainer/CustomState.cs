using PS.Patterns.Aware;

namespace PS.Shell.Module.Controls.ViewModels.BusyContainer
{
    public class CustomState : BaseNotifyPropertyChanged,
                               IMutableTitleAware
    {
        private string _title;

        public CustomState()
        {
            Title = "Mutable title";
        }

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }
    }
}
