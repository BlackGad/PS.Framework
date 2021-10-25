using System.Collections.ObjectModel;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ControlsService;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class DecimalTextBoxViewModel : BaseNotifyPropertyChanged,
                                           IControlViewModel,
                                           IViewModel
    {
        #region Constructors

        public DecimalTextBoxViewModel()
        {
            Title = "DecimalTextBox";
            Group = "Controls";

            Log = new ObservableCollection<string>();
        }

        #endregion

        #region Properties

        public ObservableCollection<string> Log { get; }

        #endregion

        #region IControlViewModel Members

        public string Group { get; }

        public string Title { get; }

        #endregion
    }
}