using System.ComponentModel;
using PS.Patterns.Aware;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface IConnectorVisual : IIsSelectedAware, INotifyPropertyChanged
    {
    }
}