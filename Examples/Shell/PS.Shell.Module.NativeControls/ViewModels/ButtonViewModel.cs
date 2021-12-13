using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    public class ButtonViewModel : BaseNotifyPropertyChanged,
                                   IViewModel
    {
        private string _content;

        #region Constructors

        public ButtonViewModel()
        {
            Content = "Content";
        }

        #endregion

        #region Properties

        public string Content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }

        #endregion
    }
}