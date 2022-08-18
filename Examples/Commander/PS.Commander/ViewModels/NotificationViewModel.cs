using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
using PS.WPF.Commands;
using PS.WPF.Patterns.Command;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class NotificationViewModel : BaseNotifyPropertyChanged,
                                         IViewModel,
                                         ITitleAware
    {
        private string _content;
        private string _title;

        public NotificationViewModel()
        {
            OKCommand = new CloseDialogCommand
            {
                Title = "OK",
                DialogResult = true,
                IsDefault = true
            };
        }

        public string Content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }

        public IUICommand OKCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }
    }
}
