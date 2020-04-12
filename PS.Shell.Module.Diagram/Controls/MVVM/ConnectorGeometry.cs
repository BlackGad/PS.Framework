using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public class ConnectorGeometry : IConnectorGeometry
    {
        #region IConnectorGeometry Members

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