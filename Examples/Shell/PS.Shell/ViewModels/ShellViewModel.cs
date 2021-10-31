using System;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ShellViewModel : BaseNotifyPropertyChanged,
                                  ITitleAware,
                                  IViewModel
    {
        private IExample _content;

        #region Constructors

        public ShellViewModel(IExamplesService examplesService)
        {
            ExamplesService = examplesService ?? throw new ArgumentNullException(nameof(examplesService));
            Title = App.GetApplicationTitle();
        }

        #endregion

        #region Properties

        public IExample Content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }

        public IExamplesService ExamplesService { get; }

        #endregion

        #region ITitleAware Members

        public string Title { get; }

        #endregion
    }
}