using System.ComponentModel;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public interface INodeGeometry : INotifyPropertyChanged
    {
        #region Properties

        double CenterX { get; set; }
        double CenterY { get; set; }

        #endregion
    }
}