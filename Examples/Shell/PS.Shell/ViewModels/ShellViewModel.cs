using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ShellViewModel : DependencyObject,
                                  IViewModel
    {
    }
}