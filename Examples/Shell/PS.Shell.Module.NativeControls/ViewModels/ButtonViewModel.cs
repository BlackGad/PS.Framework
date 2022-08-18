using System;
using NLog;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.NativeControls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ButtonViewModel : BaseNotifyPropertyChanged,
                                   IViewModel
    {
        private readonly ILogger _logger;
        private string _content;

        private bool _isEnabled;

        public ButtonViewModel(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Content = "Content";
            IsEnabled = true;
        }

        public string Content
        {
            get { return _content; }
            set
            {
                var message = $"Content changed from '{_content}' to '{value}'";
                if (SetField(ref _content, value))
                {
                    _logger.Info(message);
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                var message = $"Is enabled changed from '{_isEnabled}' to '{value}'";
                if (SetField(ref _isEnabled, value))
                {
                    _logger.Info(message);
                }
            }
        }
    }
}
