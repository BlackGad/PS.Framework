using PS.Patterns.Aware;

namespace PS.Shell.Module.Controls.ViewModels.BusyContainer
{
    public class StateWithMutableDescription : BaseNotifyPropertyChanged,
                                               IMutableDescriptionAware
    {
        private string _description;

        public StateWithMutableDescription()
        {
            Description = "Mutable description";
        }

        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }
    }
}
