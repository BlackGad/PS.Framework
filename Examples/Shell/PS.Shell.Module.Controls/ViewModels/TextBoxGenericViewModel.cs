using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.ViewModels;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class TextBoxGenericViewModel : BaseNotifyPropertyChanged,
                                           IControlViewModel,
                                           IViewModel
    {
        #region Constructors

        public TextBoxGenericViewModel()
        {
            Title = "Generic";
            Group = "TextBox";
        }

        #endregion

        #region IControlViewModel Members

        public string Group { get; }

        public string Title { get; }

        #endregion
    }
}