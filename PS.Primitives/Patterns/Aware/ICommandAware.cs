using System.Windows.Input;

namespace PS.Patterns.Aware
{
    public interface ICommandAware
    {
        ICommand Command { get; set; }
    }
}
