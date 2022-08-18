using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.WPF.Patterns.Command;

namespace PS.Shell.Module.Ribbon.ViewModels
{
    [DependencyRegisterAsSelf]
    public class RibbonViewModel : BaseNotifyPropertyChanged,
                                   IViewModel
    {
        public RibbonViewModel()
        {
            SomeCommand = new RelayUICommand();
        }

        public IUICommand SomeCommand { get; }
    }
}
