using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.NativeControls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ButtonViewModel : BaseNotifyPropertyChanged,
                                   IViewModel
    {
        private string _content;

        private bool _isEnabled;

        #region Constructors

        public ButtonViewModel()
        {
            Content = "Content";
            IsEnabled = true;
        }

        #endregion

        #region Properties

        public string Content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetField(ref _isEnabled, value); }
        }

        #endregion
    }
}