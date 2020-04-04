using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Patterns.Aware;

namespace PS.Shell.Module.Diagram.ViewModels.Nodes
{
    public class BaseNodeViewModel : IIDAware,
                                     INotifyPropertyChanged
    {
        #region IIDAware Members

        public string Id { get; set; }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Members

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}