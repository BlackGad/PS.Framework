using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.Diagram.ViewModels;

[DependencyRegisterAsSelf]
public class EditorViewModel : DependencyObject,
                               IViewModel
{
}
