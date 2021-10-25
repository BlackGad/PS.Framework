using PS.Patterns.Aware;

namespace PS.WPF.Controls.BusyContainer
{
    public class BusyState : BaseNotifyPropertyChanged,
                             IMutableTitleAware,
                             IMutableDescriptionAware
    {
        private string _description;
        private string _title;

        #region Constructors

        public BusyState()
        {
        }

        public BusyState(string title, string description = null)
        {
            Title = title;
            Description = description;
        }

        #endregion

        #region IMutableDescriptionAware Members

        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        #endregion

        #region IMutableTitleAware Members

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        #endregion

        #region Members

        public void Dispose()
        {
        }

        #endregion
    }
}