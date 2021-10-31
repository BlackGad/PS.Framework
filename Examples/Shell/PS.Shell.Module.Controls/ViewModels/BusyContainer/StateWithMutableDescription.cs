using PS.Patterns.Aware;

namespace PS.Shell.Module.Controls.ViewModels.BusyContainer
{
    public class StateWithMutableDescription : BaseNotifyPropertyChanged,
                                               IMutableDescriptionAware
    {
        private string _description;

        #region Constructors

        public StateWithMutableDescription()
        {
            Description = "Mutable description";
        }

        #endregion

        #region IMutableDescriptionAware Members

        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        #endregion
    }
}