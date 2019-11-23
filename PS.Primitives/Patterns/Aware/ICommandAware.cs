using System.Windows.Input;

namespace PS.Patterns.Aware
{
    public interface ICommandAware
    {
        #region Properties

        ICommand Command { get; set; }

        #endregion
    }
}