using PS.Patterns.Aware;

namespace PS.WPF.Controls.BusyContainer
{
    public class BusyState : BaseNotifyPropertyChanged,
                             IMutableTitleAware,
                             IMutableDescriptionAware
    {
        private string _description;
        private string _title;

        public BusyState()
        {
        }

        public BusyState(string title, string description = null)
        {
            Title = title;
            Description = description;
        }

        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        public void Dispose()
        {
        }
    }
}
